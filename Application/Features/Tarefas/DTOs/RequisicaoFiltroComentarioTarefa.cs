using DesafioMirante.Application.Common.Models;

namespace DesafioMirante.Application.Features.Tarefas.DTOs;

public sealed class RequisicaoFiltroComentarioTarefa : RequisicaoPaginada
{
    public string? Conteudo { get; set; }
    public Guid? AutorId { get; set; }
}