using DesafioMirante.Application.Common.Validators;
using DesafioMirante.Application.Features.Tarefas.DTOs;
using FluentValidation;

namespace DesafioMirante.Application.Features.Tarefas.Validators;

public sealed class ValidadorRequisicaoFiltroComentarioTarefa : ValidadorRequisicaoPaginadaBase<RequisicaoFiltroComentarioTarefa>
{
    public ValidadorRequisicaoFiltroComentarioTarefa()
    {
        RuleFor(requisicao => requisicao.Conteudo)
            .MaximumLength(200);
    }
}