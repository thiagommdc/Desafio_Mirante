namespace DesafioMirante.Domain.Common;

public abstract class EntidadeAuditavelBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CriadoEmUtc { get; set; }
    public string CriadoPor { get; set; } = string.Empty;
    public DateTime? AtualizadoEmUtc { get; set; }
    public string? AtualizadoPor { get; set; }
    public DateTime? ExcluidoEmUtc { get; set; }
    public string? ExcluidoPor { get; set; }
    public bool Excluido { get; set; }
}
