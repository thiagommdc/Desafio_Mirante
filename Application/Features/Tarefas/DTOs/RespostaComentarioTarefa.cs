namespace DesafioMirante.Application.Features.Tarefas.DTOs;

public sealed class RespostaComentarioTarefa
{
    public Guid Id { get; set; }
    public Guid TaskItemId { get; set; }
    public string Conteudo { get; set; } = string.Empty;
    public Guid? AutorId { get; set; }
    public RespostaUsuarioTarefa? Autor { get; set; }
    public DateTime CriadoEmUtc { get; set; }
    public DateTime? AtualizadoEmUtc { get; set; }
}