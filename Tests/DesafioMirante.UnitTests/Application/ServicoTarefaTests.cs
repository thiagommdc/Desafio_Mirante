using AutoMapper;
using DesafioMirante.Application.Features.Tarefas.DTOs;
using DesafioMirante.Application.Features.Tarefas.Mappings;
using DesafioMirante.Application.Features.Tarefas.Services;
using DesafioMirante.Domain.Entities;
using DesafioMirante.Domain.Enums;
using DesafioMirante.UnitTests.TestDoubles;
using Microsoft.Extensions.Logging.Abstractions;

namespace DesafioMirante.UnitTests.Application;

public sealed class ServicoTarefaTests
{
    private static readonly IMapper Mapper = new MapperConfiguration(configuracao =>
    {
        configuracao.AddProfile<PerfilMapeamentoTarefa>();
    }).CreateMapper();

    [Fact]
    public async Task CriarAsync_DeveNormalizarCamposPersistirESalvarAlteracoes()
    {
        var usuarioId = Guid.NewGuid();
        var tarefaPersistida = new TaskItem
        {
            Titulo = "Titulo normalizado",
            Descricao = "Descricao normalizada",
            UsuarioResponsavelId = usuarioId,
            UsuarioResponsavel = new User
            {
                Id = usuarioId,
                Nome = "Ana",
                Email = "ana@mirante.com"
            }
        };

        var repositorioTarefa = new FakeRepositorioTarefa();
        repositorioTarefa.OnObterDetalhadaPorIdAsync = (id, _) =>
        {
            tarefaPersistida.Id = id;
            return Task.FromResult<TaskItem?>(tarefaPersistida);
        };

        var repositorioComentario = new FakeRepositorioComentarioTarefa();
        var repositorioUsuario = new FakeRepositorioUsuario
        {
            OnExistePorIdAsync = (_, _) => Task.FromResult(true)
        };
        var unidadeTrabalho = new FakeUnidadeTrabalho();

        var servico = CriarServico(
            repositorioTarefa,
            repositorioComentario,
            repositorioUsuario,
            unidadeTrabalho);

        var requisicao = new RequisicaoCriarTarefa
        {
            Titulo = "  Titulo normalizado  ",
            Descricao = "  Descricao normalizada  ",
            Status = TaskItemStatus.InProgress,
            Prioridade = TaskItemPriority.High,
            UsuarioResponsavelId = usuarioId
        };

        var resposta = await servico.CriarAsync(requisicao, CancellationToken.None);

        Assert.NotNull(repositorioTarefa.EntidadeAdicionada);
        Assert.Equal("Titulo normalizado", repositorioTarefa.EntidadeAdicionada.Titulo);
        Assert.Equal("Descricao normalizada", repositorioTarefa.EntidadeAdicionada.Descricao);
        Assert.Equal(usuarioId, repositorioTarefa.EntidadeAdicionada.UsuarioResponsavelId);
        Assert.Equal(1, unidadeTrabalho.ChamadasSalvarAlteracoes);
        Assert.Equal(1, repositorioUsuario.ChamadasExistePorId);
        Assert.Equal("Titulo normalizado", resposta.Titulo);
        Assert.Equal("Descricao normalizada", resposta.Descricao);
        Assert.Equal(usuarioId, resposta.UsuarioResponsavelId);
    }

    [Fact]
    public async Task CriarAsync_DeveLancarExcecaoQuandoUsuarioResponsavelNaoExiste()
    {
        var repositorioTarefa = new FakeRepositorioTarefa();
        var repositorioComentario = new FakeRepositorioComentarioTarefa();
        var repositorioUsuario = new FakeRepositorioUsuario
        {
            OnExistePorIdAsync = (_, _) => Task.FromResult(false)
        };
        var unidadeTrabalho = new FakeUnidadeTrabalho();

        var servico = CriarServico(
            repositorioTarefa,
            repositorioComentario,
            repositorioUsuario,
            unidadeTrabalho);

        var requisicao = new RequisicaoCriarTarefa
        {
            Titulo = "Nova tarefa",
            UsuarioResponsavelId = Guid.NewGuid()
        };

        var excecao = await Assert.ThrowsAsync<KeyNotFoundException>(() => servico.CriarAsync(requisicao, CancellationToken.None));

        Assert.Contains("Usuario", excecao.Message);
        Assert.Null(repositorioTarefa.EntidadeAdicionada);
        Assert.Equal(0, unidadeTrabalho.ChamadasSalvarAlteracoes);
    }

    [Fact]
    public async Task AtualizarAsync_DeveAtualizarTarefaExistenteENormalizarDescricaoEmBranco()
    {
        var tarefaId = Guid.NewGuid();
        var tarefaExistente = new TaskItem
        {
            Id = tarefaId,
            Titulo = "Titulo antigo",
            Descricao = "Descricao antiga",
            Status = TaskItemStatus.Pending,
            Prioridade = TaskItemPriority.Low
        };

        var repositorioTarefa = new FakeRepositorioTarefa
        {
            OnObterDetalhadaPorIdAsync = (_, _) => Task.FromResult<TaskItem?>(tarefaExistente)
        };
        var repositorioComentario = new FakeRepositorioComentarioTarefa();
        var repositorioUsuario = new FakeRepositorioUsuario
        {
            OnExistePorIdAsync = (_, _) => Task.FromResult(true)
        };
        var unidadeTrabalho = new FakeUnidadeTrabalho();

        var servico = CriarServico(
            repositorioTarefa,
            repositorioComentario,
            repositorioUsuario,
            unidadeTrabalho);

        var requisicao = new RequisicaoAtualizarTarefa
        {
            Titulo = "  Titulo atualizado  ",
            Descricao = "   ",
            Status = TaskItemStatus.Completed,
            Prioridade = TaskItemPriority.High,
            UsuarioResponsavelId = Guid.NewGuid()
        };

        var resposta = await servico.AtualizarAsync(tarefaId, requisicao, CancellationToken.None);

        Assert.Same(tarefaExistente, repositorioTarefa.EntidadeAtualizada);
        Assert.Equal("Titulo atualizado", tarefaExistente.Titulo);
        Assert.Null(tarefaExistente.Descricao);
        Assert.Equal(TaskItemStatus.Completed, tarefaExistente.Status);
        Assert.Equal(TaskItemPriority.High, tarefaExistente.Prioridade);
        Assert.Equal(1, unidadeTrabalho.ChamadasSalvarAlteracoes);
        Assert.Equal("Titulo atualizado", resposta.Titulo);
        Assert.Null(resposta.Descricao);
    }

    [Fact]
    public async Task CriarComentarioAsync_DeveAssociarComentarioATarefaENormalizarConteudo()
    {
        var tarefaId = Guid.NewGuid();
        var autorId = Guid.NewGuid();
        var tarefa = new TaskItem
        {
            Id = tarefaId,
            Titulo = "Tarefa base"
        };
        var comentarioPersistido = new TaskComment
        {
            Conteudo = "Comentario ajustado",
            TaskItemId = tarefaId,
            AutorId = autorId
        };

        var repositorioTarefa = new FakeRepositorioTarefa
        {
            OnObterDetalhadaPorIdAsync = (_, _) => Task.FromResult<TaskItem?>(tarefa)
        };
        var repositorioComentario = new FakeRepositorioComentarioTarefa
        {
            OnObterDetalhadoPorIdAsync = (_, comentarioId, _) =>
            {
                comentarioPersistido.Id = comentarioId;
                return Task.FromResult<TaskComment?>(comentarioPersistido);
            }
        };
        var repositorioUsuario = new FakeRepositorioUsuario
        {
            OnExistePorIdAsync = (_, _) => Task.FromResult(true)
        };
        var unidadeTrabalho = new FakeUnidadeTrabalho();

        var servico = CriarServico(
            repositorioTarefa,
            repositorioComentario,
            repositorioUsuario,
            unidadeTrabalho);

        var requisicao = new RequisicaoCriarComentarioTarefa
        {
            Conteudo = "  Comentario ajustado  ",
            AutorId = autorId
        };

        var resposta = await servico.CriarComentarioAsync(tarefaId, requisicao, CancellationToken.None);

        Assert.NotNull(repositorioComentario.EntidadeAdicionada);
        Assert.Equal("Comentario ajustado", repositorioComentario.EntidadeAdicionada.Conteudo);
        Assert.Equal(tarefaId, repositorioComentario.EntidadeAdicionada.TaskItemId);
        Assert.Equal(autorId, repositorioComentario.EntidadeAdicionada.AutorId);
        Assert.Equal(1, unidadeTrabalho.ChamadasSalvarAlteracoes);
        Assert.Equal("Comentario ajustado", resposta.Conteudo);
        Assert.Equal(tarefaId, resposta.TaskItemId);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveLancarExcecaoQuandoTarefaNaoExiste()
    {
        var repositorioTarefa = new FakeRepositorioTarefa
        {
            OnObterDetalhadaPorIdAsync = (_, _) => Task.FromResult<TaskItem?>(null)
        };
        var servico = CriarServico(
            repositorioTarefa,
            new FakeRepositorioComentarioTarefa(),
            new FakeRepositorioUsuario(),
            new FakeUnidadeTrabalho());

        var excecao = await Assert.ThrowsAsync<KeyNotFoundException>(() => servico.ObterPorIdAsync(Guid.NewGuid(), CancellationToken.None));

        Assert.Contains("Tarefa", excecao.Message);
    }

    private static ServicoTarefa CriarServico(
        FakeRepositorioTarefa repositorioTarefa,
        FakeRepositorioComentarioTarefa repositorioComentarioTarefa,
        FakeRepositorioUsuario repositorioUsuario,
        FakeUnidadeTrabalho unidadeTrabalho)
    {
        return new ServicoTarefa(
            repositorioTarefa,
            repositorioComentarioTarefa,
            repositorioUsuario,
            unidadeTrabalho,
            Mapper,
            NullLogger<ServicoTarefa>.Instance);
    }
}
