using DesafioMirante.Application.Common.Models;
using DesafioMirante.Application.Features.Tarefas.DTOs;
using DesafioMirante.Application.Interfaces.Persistence;
using DesafioMirante.Domain.Entities;
using DesafioMirante.Infrastructure.Persistence.Context;
using DesafioMirante.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DesafioMirante.Infrastructure.Persistence.Repositories;

public sealed class RepositorioTarefa : Repositorio<TaskItem>, IRepositorioTarefa
{
    public RepositorioTarefa(ContextoAplicacao contexto)
        : base(contexto)
    {
    }

    public async Task<ResultadoPaginado<TaskItem>> ObterPaginadoAsync(
        RequisicaoFiltroTarefa filtro,
        CancellationToken cancellationToken)
    {
        var titulo = filtro.Titulo?.Trim();

        var consulta = Contexto.TaskItems
            .AsNoTracking()
            .TagWith("Tarefas:ObterPaginado")
            .Include(tarefa => tarefa.UsuarioResponsavel)
            .AplicarFiltroQuando(
                !string.IsNullOrWhiteSpace(titulo),
                origem => origem.Where(tarefa => EF.Functions.Like(tarefa.Titulo, $"%{titulo}%")))
            .AplicarFiltroQuando(
                filtro.Status.HasValue,
                origem => origem.Where(tarefa => tarefa.Status == filtro.Status!.Value))
            .AplicarFiltroQuando(
                filtro.Prioridade.HasValue,
                origem => origem.Where(tarefa => tarefa.Prioridade == filtro.Prioridade!.Value))
            .AplicarFiltroQuando(
                filtro.UsuarioResponsavelId.HasValue,
                origem => origem.Where(tarefa => tarefa.UsuarioResponsavelId == filtro.UsuarioResponsavelId!.Value))
            .OrderBy(tarefa => tarefa.Status)
            .ThenByDescending(tarefa => tarefa.Prioridade)
            .ThenBy(tarefa => tarefa.Titulo)
            .ThenBy(tarefa => tarefa.Id);

        return await consulta.ParaResultadoPaginadoAsync(filtro, cancellationToken);
    }

    public async Task<TaskItem?> ObterDetalhadaPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Contexto.TaskItems
            .AsNoTracking()
            .Include(tarefa => tarefa.UsuarioResponsavel)
            .FirstOrDefaultAsync(tarefa => tarefa.Id == id, cancellationToken);
    }
}