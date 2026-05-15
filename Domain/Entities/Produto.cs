using DesafioMirante.Domain.Common;

namespace DesafioMirante.Domain.Entities;

public sealed class Produto : EntidadeAuditavelBase
{
    public string Nome { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int QuantidadeEstoque { get; set; }
    public bool Ativo { get; set; } = true;
}
