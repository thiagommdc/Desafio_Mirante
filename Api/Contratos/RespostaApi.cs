namespace DesafioMirante.Api.Contratos;

public class RespostaApi<T>
{
    public bool Sucesso { get; init; }
    public string Mensagem { get; init; } = string.Empty;
    public T? Dados { get; init; }
    public object? Metadados { get; init; }
    public string TraceId { get; init; } = string.Empty;
    public DateTime TimestampUtc { get; init; } = DateTime.UtcNow;

    public static RespostaApi<T> CriarSucesso(
        T? dados,
        string mensagem,
        string traceId,
        object? metadados = null)
    {
        return new RespostaApi<T>
        {
            Sucesso = true,
            Mensagem = mensagem,
            Dados = dados,
            Metadados = metadados,
            TraceId = traceId,
            TimestampUtc = DateTime.UtcNow
        };
    }
}
