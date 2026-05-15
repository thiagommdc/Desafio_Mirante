using DesafioMirante.Application.Common.Models;
using DesafioMirante.Application.Features.Tarefas.DTOs;
using DesafioMirante.Domain.Entities;

namespace DesafioMirante.Application.Interfaces.Persistence;

public interface IRepositorioComentarioTarefa : IRepositorioGenerico<TaskComment>
{
    Task<ResultadoPaginado<TaskComment>> ObterPaginadoPorTarefaAsync(Guid tarefaId, RequisicaoFiltroComentarioTarefa filtro, CancellationToken cancellationToken);
    Task<TaskComment?> ObterDetalhadoPorIdAsync(Guid tarefaId, Guid comentarioId, CancellationToken cancellationToken);
}