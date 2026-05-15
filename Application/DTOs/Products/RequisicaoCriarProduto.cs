namespace DesafioMirante.Application.DTOs.Products;

public sealed class RequisicaoCriarProduto
{
    public string Nome { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int QuantidadeEstoque { get; set; }
    public bool Ativo { get; set; } = true;
}
