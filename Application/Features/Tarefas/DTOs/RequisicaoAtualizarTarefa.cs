using DesafioMirante.Domain.Enums;

namespace DesafioMirante.Application.Features.Tarefas.DTOs;

public sealed class RequisicaoAtualizarTarefa
{
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public TaskItemStatus Status { get; set; }
    public TaskItemPriority Prioridade { get; set; }
    public Guid? UsuarioResponsavelId { get; set; }
}