namespace DesafioMirante.Api.Contratos;

public sealed class RespostaErroApi
{
    public bool Sucesso { get; init; }
    public string Mensagem { get; init; } = string.Empty;
    public object? Erros { get; init; }
    public string TraceId { get; init; } = string.Empty;
    public DateTime TimestampUtc { get; init; } = DateTime.UtcNow;

    public static RespostaErroApi Criar(
        string mensagem,
        string traceId,
        object? erros = null)
    {
        return new RespostaErroApi
        {
            Sucesso = false,
            Mensagem = mensagem,
            Erros = erros,
            TraceId = traceId,
            TimestampUtc = DateTime.UtcNow
        };
    }
}
