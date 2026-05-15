using DesafioMirante.Domain.Enums;

namespace DesafioMirante.Application.Features.Tarefas.DTOs;

public sealed class RequisicaoCriarTarefa
{
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;
    public TaskItemPriority Prioridade { get; set; } = TaskItemPriority.Medium;
    public Guid? UsuarioResponsavelId { get; set; }
}