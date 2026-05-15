namespace DesafioMirante.Application.Features.Tarefas.DTOs;

public sealed class RespostaUsuarioTarefa
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}