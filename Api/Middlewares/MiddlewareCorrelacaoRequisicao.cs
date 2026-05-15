namespace DesafioMirante.Api.Middlewares;

public sealed class MiddlewareCorrelacaoRequisicao
{
    public const string NomeCabecalhoCorrelacao = "X-Correlation-Id";

    private readonly RequestDelegate _next;
    private readonly ILogger<MiddlewareCorrelacaoRequisicao> _logger;

    public MiddlewareCorrelacaoRequisicao(
        RequestDelegate next,
        ILogger<MiddlewareCorrelacaoRequisicao> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = ObterOuGerarCorrelationId(context);

        context.TraceIdentifier = correlationId;
        context.Response.Headers[NomeCabecalhoCorrelacao] = correlationId;

        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = correlationId,
            ["TraceId"] = context.TraceIdentifier
        }))
        {
            await _next(context);
        }
    }

    private static string ObterOuGerarCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(NomeCabecalhoCorrelacao, out var correlationId)
            && !string.IsNullOrWhiteSpace(correlationId))
        {
            return correlationId.ToString();
        }

        return Guid.NewGuid().ToString("N");
    }
}
