namespace DesafioMirante.Application.Features.Produtos.DTOs;

public sealed class RespostaProduto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int QuantidadeEstoque { get; set; }
    public bool Ativo { get; set; }
    public DateTime CriadoEmUtc { get; set; }
    public DateTime? AtualizadoEmUtc { get; set; }
}
