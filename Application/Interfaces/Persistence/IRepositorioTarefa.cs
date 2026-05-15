using DesafioMirante.Application.Common.Models;
using DesafioMirante.Application.Features.Tarefas.DTOs;
using DesafioMirante.Domain.Entities;

namespace DesafioMirante.Application.Interfaces.Persistence;

public interface IRepositorioTarefa : IRepositorioGenerico<TaskItem>
{
    Task<ResultadoPaginado<TaskItem>> ObterPaginadoAsync(RequisicaoFiltroTarefa filtro, CancellationToken cancellationToken);
    Task<TaskItem?> ObterDetalhadaPorIdAsync(Guid id, CancellationToken cancellationToken);
}