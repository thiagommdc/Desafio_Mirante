using DesafioMirante.Application.Common.Models;
using FluentValidation;

namespace DesafioMirante.Application.Common.Validators;

public abstract class ValidadorRequisicaoPaginadaBase<TRequisicao> : AbstractValidator<TRequisicao>
    where TRequisicao : RequisicaoPaginada
{
    protected ValidadorRequisicaoPaginadaBase()
    {
        RuleFor(requisicao => requisicao.NumeroPagina)
            .GreaterThan(0);

        RuleFor(requisicao => requisicao.TamanhoPagina)
            .InclusiveBetween(1, 100);
    }
}
