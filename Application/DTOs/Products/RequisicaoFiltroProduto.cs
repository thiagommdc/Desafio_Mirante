using DesafioMirante.Application.Common.Models;

namespace DesafioMirante.Application.DTOs.Products;

public sealed class RequisicaoFiltroProduto : RequisicaoPaginada
{
    public string? TermoBusca { get; set; }
    public decimal? PrecoMinimo { get; set; }
    public decimal? PrecoMaximo { get; set; }
    public bool? Ativo { get; set; }
}
