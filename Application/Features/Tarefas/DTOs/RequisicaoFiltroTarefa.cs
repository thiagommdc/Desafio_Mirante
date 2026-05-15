using DesafioMirante.Application.Common.Models;
using DesafioMirante.Domain.Enums;

namespace DesafioMirante.Application.Features.Tarefas.DTOs;

public sealed class RequisicaoFiltroTarefa : RequisicaoPaginada
{
    public string? Titulo { get; set; }
    public TaskItemStatus? Status { get; set; }
    public TaskItemPriority? Prioridade { get; set; }
    public Guid? UsuarioResponsavelId { get; set; }
}