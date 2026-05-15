using DesafioMirante.Domain.Common;

namespace DesafioMirante.Domain.Entities;

public sealed class User : EntidadeAuditavelBase
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ICollection<TaskItem> TarefasResponsaveis { get; set; } = new List<TaskItem>();
    public ICollection<TaskComment> Comentarios { get; set; } = new List<TaskComment>();
}