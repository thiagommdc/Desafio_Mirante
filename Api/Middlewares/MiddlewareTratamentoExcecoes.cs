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
            var erros = RespostaErroApiFactory.CriarErrosValidacao(exception.Errors);
            RegistrarErro(
                context,
                exception,
                LogLevel.Warning,
                StatusCodes.Status400BadRequest,
                "Erro de validacao ao processar a requisicao.",
                erros.Keys);

            await EscreverRespostaErroAsync(
                context,
                StatusCodes.Status400BadRequest,
                "Um ou mais erros de validacao ocorreram.",
                erros);
        }
        catch (BadHttpRequestException exception)
        {
            RegistrarErro(
                context,
                exception,
                LogLevel.Warning,
                StatusCodes.Status400BadRequest,
                "Requisicao HTTP malformada.",
                []);

            await EscreverRespostaErroAsync(
                context,
                StatusCodes.Status400BadRequest,
                "A requisicao HTTP e invalida.");
        }
        catch (KeyNotFoundException exception)
        {
            RegistrarErro(
                context,
                exception,
                LogLevel.Warning,
                StatusCodes.Status404NotFound,
                "Recurso nao encontrado para a requisicao.",
                []);

            await EscreverRespostaErroAsync(
                context,
                StatusCodes.Status404NotFound,
                exception.Message);
        }
        catch (InvalidOperationException exception)
        {
            RegistrarErro(
                context,
                exception,
                LogLevel.Warning,
                StatusCodes.Status409Conflict,
                "Violacao de regra de negocio para a requisicao.",
                []);

            await EscreverRespostaErroAsync(
                context,
                StatusCodes.Status409Conflict,
                exception.Message);
        }
        catch (Exception exception)
        {
            RegistrarErro(
                context,
                exception,
                LogLevel.Error,
                StatusCodes.Status500InternalServerError,
                "Erro nao tratado ao processar a requisicao.",
                []);

            await EscreverRespostaErroAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "Ocorreu um erro inesperado.");
        }
    }

    private void RegistrarErro(
        HttpContext context,
        Exception exception,
        LogLevel nivelLog,
        int statusCode,
        string mensagem,
        IEnumerable<string> camposInvalidos)
    {
        _logger.Log(
            nivelLog,
            exception,
            "{Mensagem} Metodo: {Metodo}, Caminho: {Caminho}, StatusCode: {StatusCode}, TraceId: {TraceId}, CamposInvalidos: {CamposInvalidos}",
            mensagem,
            context.Request.Method,
            context.Request.Path,
            statusCode,
            context.TraceIdentifier,
            camposInvalidos);
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
