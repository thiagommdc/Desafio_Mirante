using FluentValidation;

namespace DesafioMirante.Application.Common.Validators;

public abstract class ValidadorProdutoBase<TRequisicao> : AbstractValidator<TRequisicao>
    where TRequisicao : class
{
    protected void ConfigurarValidacoesProduto(
        Func<TRequisicao, string> obterNome,
        Func<TRequisicao, string> obterSku,
        Func<TRequisicao, string?> obterDescricao,
        Func<TRequisicao, decimal> obterPreco,
        Func<TRequisicao, int> obterQuantidadeEstoque)
    {
        RuleFor(requisicao => obterNome(requisicao))
            .NotEmpty()
            .Must(nome => !string.IsNullOrWhiteSpace(nome))
            .WithMessage("Nome e obrigatorio.")
            .MaximumLength(120);

        RuleFor(requisicao => obterSku(requisicao))
            .NotEmpty()
            .Must(sku => !string.IsNullOrWhiteSpace(sku))
            .WithMessage("Sku e obrigatorio.")
            .MaximumLength(40);

        RuleFor(requisicao => obterDescricao(requisicao))
            .MaximumLength(500);

        RuleFor(requisicao => obterPreco(requisicao))
            .GreaterThan(0);

        RuleFor(requisicao => obterQuantidadeEstoque(requisicao))
            .GreaterThanOrEqualTo(0);
    }
}
