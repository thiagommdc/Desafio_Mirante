using DesafioMirante.Application.Common.Validators;
using DesafioMirante.Application.Features.Produtos.DTOs;

namespace DesafioMirante.Application.Features.Produtos.Validators;

public sealed class ValidadorRequisicaoAtualizarProduto : ValidadorProdutoBase<RequisicaoAtualizarProduto>
{
    public ValidadorRequisicaoAtualizarProduto()
    {
        ConfigurarValidacoesProduto(
            requisicao => requisicao.Nome,
            requisicao => requisicao.Sku,
            requisicao => requisicao.Descricao,
            requisicao => requisicao.Preco,
            requisicao => requisicao.QuantidadeEstoque);
    }
}
