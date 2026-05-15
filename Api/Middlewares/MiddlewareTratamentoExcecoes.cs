using DesafioMirante.Api.Contratos;
using FluentValidation;

namespace DesafioMirante.Api.Middlewares;

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

            await EscreverRespostaErroAsync(
                context,
                StatusCodes.Status400BadRequest,
                "Um ou mais erros de validacao ocorreram.",
                erros);
        }
        catch (KeyNotFoundException exception)
        {
            _logger.LogWarning(exception, "Recurso nao encontrado para a requisicao {Path}", context.Request.Path);

            await EscreverRespostaErroAsync(
                context,
                StatusCodes.Status404NotFound,
                exception.Message);
        }
        catch (InvalidOperationException exception)
        {
            _logger.LogWarning(exception, "Violacao de regra de negocio para a requisicao {Path}", context.Request.Path);

            await EscreverRespostaErroAsync(
                context,
                StatusCodes.Status409Conflict,
                exception.Message);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Erro nao tratado ao processar a requisicao {Path}", context.Request.Path);

            await EscreverRespostaErroAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "Ocorreu um erro inesperado.");
        }
    }

    private static async Task EscreverRespostaErroAsync(
        HttpContext context,
        int statusCode,
        string detail,
        object? erros = null)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.Clear();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var resposta = RespostaErroApi.Criar(detail, context.TraceIdentifier, erros);
        await context.Response.WriteAsJsonAsync(resposta);
    }
}
