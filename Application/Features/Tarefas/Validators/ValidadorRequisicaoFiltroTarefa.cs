using DesafioMirante.Application.Common.Validators;
using DesafioMirante.Application.Features.Tarefas.DTOs;
using FluentValidation;

namespace DesafioMirante.Application.Features.Tarefas.Validators;

public sealed class ValidadorRequisicaoFiltroTarefa : ValidadorRequisicaoPaginadaBase<RequisicaoFiltroTarefa>
{
    public ValidadorRequisicaoFiltroTarefa()
    {
        RuleFor(requisicao => requisicao.Titulo)
            .MaximumLength(200);
    }
}