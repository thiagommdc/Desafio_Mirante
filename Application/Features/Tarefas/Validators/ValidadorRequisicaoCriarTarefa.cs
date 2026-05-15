using DesafioMirante.Application.Features.Tarefas.DTOs;
using FluentValidation;

namespace DesafioMirante.Application.Features.Tarefas.Validators;

public sealed class ValidadorRequisicaoCriarTarefa : AbstractValidator<RequisicaoCriarTarefa>
{
    public ValidadorRequisicaoCriarTarefa()
    {
        RuleFor(requisicao => requisicao.Titulo)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(requisicao => requisicao.Descricao)
            .MaximumLength(2000);

        RuleFor(requisicao => requisicao.Status)
            .IsInEnum();

        RuleFor(requisicao => requisicao.Prioridade)
            .IsInEnum();
    }
}