using DesafioMirante.Application.Common.Models;
using DesafioMirante.Application.Features.Tarefas.DTOs;
using DesafioMirante.Application.Interfaces.Persistence;
using DesafioMirante.Domain.Entities;
using DesafioMirante.Infrastructure.Persistence.Context;
using DesafioMirante.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DesafioMirante.Infrastructure.Persistence.Repositories;

public sealed class RepositorioComentarioTarefa : Repositorio<TaskComment>, IRepositorioComentarioTarefa
{
    public RepositorioComentarioTarefa(ContextoAplicacao contexto)
        : base(contexto)
    {
    }

    public async Task<ResultadoPaginado<TaskComment>> ObterPaginadoPorTarefaAsync(
        Guid tarefaId,
        RequisicaoFiltroComentarioTarefa filtro,
        CancellationToken cancellationToken)
    {
        var conteudo = filtro.Conteudo?.Trim();

        var consulta = Contexto.TaskComments
            .AsNoTracking()
            .TagWith("ComentariosTarefa:ObterPaginado")
            .Include(comentario => comentario.Autor)
            .Where(comentario => comentario.TaskItemId == tarefaId)
            .AplicarFiltroQuando(
                !string.IsNullOrWhiteSpace(conteudo),
                origem => origem.Where(comentario => EF.Functions.Like(comentario.Conteudo, $"%{conteudo}%")))
            .AplicarFiltroQuando(
                filtro.AutorId.HasValue,
                origem => origem.Where(comentario => comentario.AutorId == filtro.AutorId!.Value))
            .OrderByDescending(comentario => comentario.CriadoEmUtc)
            .ThenBy(comentario => comentario.Id);

        return await consulta.ParaResultadoPaginadoAsync(filtro, cancellationToken);
    }

    public async Task<TaskComment?> ObterDetalhadoPorIdAsync(
        Guid tarefaId,
        Guid comentarioId,
        CancellationToken cancellationToken)
    {
        return await Contexto.TaskComments
            .AsNoTracking()
            .Include(comentario => comentario.Autor)
            .FirstOrDefaultAsync(
                comentario => comentario.TaskItemId == tarefaId && comentario.Id == comentarioId,
                cancellationToken);
    }
}