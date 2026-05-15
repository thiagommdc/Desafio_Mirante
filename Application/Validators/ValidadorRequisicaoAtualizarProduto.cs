using DesafioMirante.Application.DTOs.Products;
using FluentValidation;

namespace DesafioMirante.Application.Validators;

public sealed class ValidadorRequisicaoAtualizarProduto : AbstractValidator<RequisicaoAtualizarProduto>
{
    public ValidadorRequisicaoAtualizarProduto()
    {
        RuleFor(requisicao => requisicao.Nome)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(requisicao => requisicao.Sku)
            .NotEmpty()
            .MaximumLength(40);

        RuleFor(requisicao => requisicao.Descricao)
            .MaximumLength(500);

        RuleFor(requisicao => requisicao.Preco)
            .GreaterThan(0);

        RuleFor(requisicao => requisicao.QuantidadeEstoque)
            .GreaterThanOrEqualTo(0);
    }
}
