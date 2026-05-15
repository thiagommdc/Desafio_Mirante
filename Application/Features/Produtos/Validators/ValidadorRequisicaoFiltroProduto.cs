using DesafioMirante.Application.Common.Validators;
using DesafioMirante.Application.Features.Produtos.DTOs;
using FluentValidation;

namespace DesafioMirante.Application.Features.Produtos.Validators;

public sealed class ValidadorRequisicaoFiltroProduto : ValidadorRequisicaoPaginadaBase<RequisicaoFiltroProduto>
{
    public ValidadorRequisicaoFiltroProduto()
    {
        RuleFor(requisicao => requisicao.Nome)
            .MaximumLength(120);
    }
}
