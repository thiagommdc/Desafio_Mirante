namespace DesafioMirante.Application.Features.Tarefas.DTOs;

public sealed class RequisicaoAtualizarComentarioTarefa
{
    public string Conteudo { get; set; } = string.Empty;
    public Guid? AutorId { get; set; }
}