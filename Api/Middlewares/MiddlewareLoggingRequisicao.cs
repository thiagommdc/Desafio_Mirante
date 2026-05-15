using System.Diagnostics;

namespace DesafioMirante.Api.Middlewares;

public sealed class MiddlewareLoggingRequisicao
{
    private readonly RequestDelegate _next;
    private readonly ILogger<MiddlewareLoggingRequisicao> _logger;

    public MiddlewareLoggingRequisicao(
        RequestDelegate next,
        ILogger<MiddlewareLoggingRequisicao> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            _logger.LogInformation(
                "Requisicao HTTP processada. Metodo: {Metodo}, Caminho: {Caminho}, StatusCode: {StatusCode}, DuracaoMs: {DuracaoMs}, TraceId: {TraceId}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                context.TraceIdentifier);
        }
    }
}
