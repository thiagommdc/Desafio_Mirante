using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DesafioMirante.Api.Middleware;

public sealed class MiddlewareTratamentoExcecoes
{
    private readonly RequestDelegate _next;
    private readonly ILogger<MiddlewareTratamentoExcecoes> _logger;

    public MiddlewareTratamentoExcecoes(
        RequestDelegate next,
        ILogger<MiddlewareTratamentoExcecoes> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException exception)
        {
            _logger.LogWarning(exception, "Erro de validacao ao processar a requisicao {Path}", context.Request.Path);

            var erros = exception.Errors
                .GroupBy(erro => erro.PropertyName)
                .ToDictionary(
                    grupo => grupo.Key,
                    grupo => grupo.Select(erro => erro.ErrorMessage).ToArray());

            await EscreverDetalhesProblemaAsync(
                context,
                StatusCodes.Status400BadRequest,
                "Erro de validacao",
                "Um ou mais erros de validacao ocorreram.",
                new Dictionary<string, object?>
                {
                    ["erros"] = erros
                });
        }
        catch (KeyNotFoundException exception)
        {
            _logger.LogWarning(exception, "Recurso nao encontrado para a requisicao {Path}", context.Request.Path);

            await EscreverDetalhesProblemaAsync(
                context,
                StatusCodes.Status404NotFound,
                "Recurso nao encontrado",
                exception.Message);
        }
        catch (InvalidOperationException exception)
        {
            _logger.LogWarning(exception, "Violacao de regra de negocio para a requisicao {Path}", context.Request.Path);

            await EscreverDetalhesProblemaAsync(
                context,
                StatusCodes.Status409Conflict,
                "Violacao de regra de negocio",
                exception.Message);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Erro nao tratado ao processar a requisicao {Path}", context.Request.Path);

            await EscreverDetalhesProblemaAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "Erro interno do servidor",
                "Ocorreu um erro inesperado.");
        }
    }

    private static async Task EscreverDetalhesProblemaAsync(
        HttpContext context,
        int statusCode,
        string title,
        string detail,
        IDictionary<string, object?>? extensoes = null)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.Clear();
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        var detalhesProblema = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        detalhesProblema.Extensions["traceId"] = context.TraceIdentifier;

        if (extensoes is not null)
        {
            foreach (var extensao in extensoes)
            {
                detalhesProblema.Extensions[extensao.Key] = extensao.Value;
            }
        }

        await context.Response.WriteAsJsonAsync(detalhesProblema);
    }
}
