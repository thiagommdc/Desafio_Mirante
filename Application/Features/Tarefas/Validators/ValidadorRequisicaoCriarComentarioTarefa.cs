using DesafioMirante.Application.Features.Tarefas.DTOs;
using FluentValidation;

namespace DesafioMirante.Application.Features.Tarefas.Validators;

public sealed class ValidadorRequisicaoCriarComentarioTarefa : AbstractValidator<RequisicaoCriarComentarioTarefa>
{
    public ValidadorRequisicaoCriarComentarioTarefa()
    {
        RuleFor(requisicao => requisicao.Conteudo)
            .NotEmpty()
            .MaximumLength(1000);
    }
}