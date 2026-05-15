using DesafioMirante.Application.Common.Models;

namespace DesafioMirante.Application.Features.Produtos.DTOs;

public sealed class RequisicaoFiltroProduto : RequisicaoPaginada
{
    public string? Nome { get; set; }
    public bool? Ativo { get; set; }
}
