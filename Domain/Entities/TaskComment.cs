using DesafioMirante.Domain.Common;

namespace DesafioMirante.Domain.Entities;

public sealed class TaskComment : EntidadeAuditavelBase
{
    public string Conteudo { get; set; } = string.Empty;
    public Guid TaskItemId { get; set; }
    public TaskItem TaskItem { get; set; } = null!;
    public Guid? AutorId { get; set; }
    public User? Autor { get; set; }
}