using DesafioMirante.Application.Features.Tarefas.DTOs;
using FluentValidation;

namespace DesafioMirante.Application.Features.Tarefas.Validators;

public sealed class ValidadorRequisicaoAtualizarComentarioTarefa : AbstractValidator<RequisicaoAtualizarComentarioTarefa>
{
    public ValidadorRequisicaoAtualizarComentarioTarefa()
    {
        RuleFor(requisicao => requisicao.Conteudo)
            .NotEmpty()
            .MaximumLength(1000);
    }
}