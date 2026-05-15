using DesafioMirante.Domain.Enums;

namespace DesafioMirante.Application.Features.Tarefas.DTOs;

public sealed class RespostaTarefa
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public TaskItemStatus Status { get; set; }
    public TaskItemPriority Prioridade { get; set; }
    public Guid? UsuarioResponsavelId { get; set; }
    public RespostaUsuarioTarefa? UsuarioResponsavel { get; set; }
    public DateTime CriadoEmUtc { get; set; }
    public DateTime? AtualizadoEmUtc { get; set; }
}