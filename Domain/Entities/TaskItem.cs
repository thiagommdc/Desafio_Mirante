using DesafioMirante.Domain.Common;
using DesafioMirante.Domain.Enums;

namespace DesafioMirante.Domain.Entities;

public sealed class TaskItem : EntidadeAuditavelBase
{
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;
    public TaskItemPriority Prioridade { get; set; } = TaskItemPriority.Medium;
    public Guid? UsuarioResponsavelId { get; set; }
    public User? UsuarioResponsavel { get; set; }
    public ICollection<TaskComment> Comentarios { get; set; } = new List<TaskComment>();
}