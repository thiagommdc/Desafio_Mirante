using DesafioMirante.Application.DTOs.Products;
using FluentValidation;

namespace DesafioMirante.Application.Validators;

public sealed class ValidadorRequisicaoFiltroProduto : AbstractValidator<RequisicaoFiltroProduto>
{
    public ValidadorRequisicaoFiltroProduto()
    {
        RuleFor(requisicao => requisicao.NumeroPagina)
            .GreaterThan(0);

        RuleFor(requisicao => requisicao.TamanhoPagina)
            .InclusiveBetween(1, 100);

        RuleFor(requisicao => requisicao.PrecoMinimo)
            .GreaterThanOrEqualTo(0)
            .When(requisicao => requisicao.PrecoMinimo.HasValue);

        RuleFor(requisicao => requisicao.PrecoMaximo)
            .GreaterThanOrEqualTo(0)
            .When(requisicao => requisicao.PrecoMaximo.HasValue);

        RuleFor(requisicao => requisicao)
            .Must(requisicao => !requisicao.PrecoMinimo.HasValue || !requisicao.PrecoMaximo.HasValue || requisicao.PrecoMinimo <= requisicao.PrecoMaximo)
            .WithMessage("PrecoMinimo nao pode ser maior que PrecoMaximo.");
    }
}
