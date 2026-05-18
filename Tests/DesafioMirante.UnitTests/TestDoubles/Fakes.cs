using DesafioMirante.Application.Common.Models;
using DesafioMirante.Application.Features.Tarefas.DTOs;
using DesafioMirante.Application.Interfaces.Persistence;
using DesafioMirante.Domain.Entities;

namespace DesafioMirante.UnitTests.TestDoubles;

internal sealed class FakeRepositorioTarefa : IRepositorioTarefa
{
    public Func<Guid, CancellationToken, Task<TaskItem?>>? OnObterPorIdAsync { get; set; }
    public Func<RequisicaoFiltroTarefa, CancellationToken, Task<ResultadoPaginado<TaskItem>>>? OnObterPaginadoAsync { get; set; }
    public Func<Guid, CancellationToken, Task<TaskItem?>>? OnObterDetalhadaPorIdAsync { get; set; }

    public TaskItem? EntidadeAdicionada { get; private set; }
    public TaskItem? EntidadeAtualizada { get; private set; }
    public TaskItem? EntidadeRemovida { get; private set; }

    public Task<TaskItem?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
        => OnObterPorIdAsync?.Invoke(id, cancellationToken) ?? Task.FromResult<TaskItem?>(null);

    public Task AdicionarAsync(TaskItem entidade, CancellationToken cancellationToken)
    {
        EntidadeAdicionada = entidade;
        return Task.CompletedTask;
    }

    public void Atualizar(TaskItem entidade)
    {
        EntidadeAtualizada = entidade;
    }

    public void Remover(TaskItem entidade)
    {
        EntidadeRemovida = entidade;
    }

    public Task<ResultadoPaginado<TaskItem>> ObterPaginadoAsync(RequisicaoFiltroTarefa filtro, CancellationToken cancellationToken)
        => OnObterPaginadoAsync?.Invoke(filtro, cancellationToken)
           ?? Task.FromResult(new ResultadoPaginado<TaskItem>(Array.Empty<TaskItem>(), 1, 10, 0));

    public Task<TaskItem?> ObterDetalhadaPorIdAsync(Guid id, CancellationToken cancellationToken)
        => OnObterDetalhadaPorIdAsync?.Invoke(id, cancellationToken) ?? Task.FromResult<TaskItem?>(null);
}

internal sealed class FakeRepositorioComentarioTarefa : IRepositorioComentarioTarefa
{
    public Func<Guid, CancellationToken, Task<TaskComment?>>? OnObterPorIdAsync { get; set; }
    public Func<Guid, RequisicaoFiltroComentarioTarefa, CancellationToken, Task<ResultadoPaginado<TaskComment>>>? OnObterPaginadoPorTarefaAsync { get; set; }
    public Func<Guid, Guid, CancellationToken, Task<TaskComment?>>? OnObterDetalhadoPorIdAsync { get; set; }

    public TaskComment? EntidadeAdicionada { get; private set; }
    public TaskComment? EntidadeAtualizada { get; private set; }
    public TaskComment? EntidadeRemovida { get; private set; }

    public Task<TaskComment?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
        => OnObterPorIdAsync?.Invoke(id, cancellationToken) ?? Task.FromResult<TaskComment?>(null);

    public Task AdicionarAsync(TaskComment entidade, CancellationToken cancellationToken)
    {
        EntidadeAdicionada = entidade;
        return Task.CompletedTask;
    }

    public void Atualizar(TaskComment entidade)
    {
        EntidadeAtualizada = entidade;
    }

    public void Remover(TaskComment entidade)
    {
        EntidadeRemovida = entidade;
    }

    public Task<ResultadoPaginado<TaskComment>> ObterPaginadoPorTarefaAsync(
        Guid tarefaId,
        RequisicaoFiltroComentarioTarefa filtro,
        CancellationToken cancellationToken)
        => OnObterPaginadoPorTarefaAsync?.Invoke(tarefaId, filtro, cancellationToken)
           ?? Task.FromResult(new ResultadoPaginado<TaskComment>(Array.Empty<TaskComment>(), 1, 10, 0));

    public Task<TaskComment?> ObterDetalhadoPorIdAsync(Guid tarefaId, Guid comentarioId, CancellationToken cancellationToken)
        => OnObterDetalhadoPorIdAsync?.Invoke(tarefaId, comentarioId, cancellationToken) ?? Task.FromResult<TaskComment?>(null);
}

internal sealed class FakeRepositorioUsuario : IRepositorioUsuario
{
    public Func<Guid, CancellationToken, Task<User?>>? OnObterPorIdAsync { get; set; }
    public Func<Guid, CancellationToken, Task<bool>>? OnExistePorIdAsync { get; set; }

    public User? EntidadeAdicionada { get; private set; }
    public User? EntidadeAtualizada { get; private set; }
    public User? EntidadeRemovida { get; private set; }
    public int ChamadasExistePorId { get; private set; }

    public Task<User?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
        => OnObterPorIdAsync?.Invoke(id, cancellationToken) ?? Task.FromResult<User?>(null);

    public Task AdicionarAsync(User entidade, CancellationToken cancellationToken)
    {
        EntidadeAdicionada = entidade;
        return Task.CompletedTask;
    }

    public void Atualizar(User entidade)
    {
        EntidadeAtualizada = entidade;
    }

    public void Remover(User entidade)
    {
        EntidadeRemovida = entidade;
    }

    public Task<bool> ExistePorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        ChamadasExistePorId++;
        return OnExistePorIdAsync?.Invoke(id, cancellationToken) ?? Task.FromResult(false);
    }
}

internal sealed class FakeUnidadeTrabalho : IUnidadeTrabalho
{
    public int ChamadasSalvarAlteracoes { get; private set; }

    public Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken)
    {
        ChamadasSalvarAlteracoes++;
        return Task.FromResult(1);
    }
}
