using AutoMapper;
using DesafioMirante.Application.Common.Models;
using DesafioMirante.Application.Features.Tarefas.DTOs;
using DesafioMirante.Application.Interfaces.Persistence;
using DesafioMirante.Application.Interfaces.Services;
using DesafioMirante.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DesafioMirante.Application.Features.Tarefas.Services;

public sealed class ServicoTarefa : IServicoTarefa
{
    private readonly IRepositorioTarefa _repositorioTarefa;
    private readonly IRepositorioComentarioTarefa _repositorioComentarioTarefa;
    private readonly IRepositorioUsuario _repositorioUsuario;
    private readonly IUnidadeTrabalho _unidadeTrabalho;
    private readonly IMapper _mapeador;
    private readonly ILogger<ServicoTarefa> _logger;

    public ServicoTarefa(
        IRepositorioTarefa repositorioTarefa,
        IRepositorioComentarioTarefa repositorioComentarioTarefa,
        IRepositorioUsuario repositorioUsuario,
        IUnidadeTrabalho unidadeTrabalho,
        IMapper mapeador,
        ILogger<ServicoTarefa> logger)
    {
        _repositorioTarefa = repositorioTarefa;
        _repositorioComentarioTarefa = repositorioComentarioTarefa;
        _repositorioUsuario = repositorioUsuario;
        _unidadeTrabalho = unidadeTrabalho;
        _mapeador = mapeador;
        _logger = logger;
    }

    public async Task<ResultadoPaginado<RespostaTarefa>> ObterPaginadoAsync(
        RequisicaoFiltroTarefa filtro,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Consultando tarefas paginadas. Pagina: {NumeroPagina}, Tamanho: {TamanhoPagina}, Titulo: {Titulo}, Status: {Status}, Prioridade: {Prioridade}, UsuarioResponsavelId: {UsuarioResponsavelId}",
            filtro.NumeroPagina,
            filtro.TamanhoPagina,
            filtro.Titulo,
            filtro.Status,
            filtro.Prioridade,
            filtro.UsuarioResponsavelId);

        var tarefas = await _repositorioTarefa.ObterPaginadoAsync(filtro, cancellationToken);
        var itens = _mapeador.Map<IReadOnlyCollection<RespostaTarefa>>(tarefas.Itens);

        return new ResultadoPaginado<RespostaTarefa>(
            itens,
            tarefas.NumeroPagina,
            tarefas.TamanhoPagina,
            tarefas.TotalRegistros);
    }

    public async Task<RespostaTarefa> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Consultando tarefa {TarefaId}", id);

        var tarefa = await ObterTarefaOuLancarExcecaoAsync(id, cancellationToken);
        return _mapeador.Map<RespostaTarefa>(tarefa);
    }

    public async Task<RespostaTarefa> CriarAsync(
        RequisicaoCriarTarefa requisicao,
        CancellationToken cancellationToken)
    {
        NormalizarRequisicao(requisicao);
        await ValidarUsuarioAsync(requisicao.UsuarioResponsavelId, cancellationToken);

        var tarefa = _mapeador.Map<TaskItem>(requisicao);

        await _repositorioTarefa.AdicionarAsync(tarefa, cancellationToken);
        await _unidadeTrabalho.SalvarAlteracoesAsync(cancellationToken);

        _logger.LogInformation("Tarefa {TarefaId} criada", tarefa.Id);

        var tarefaCriada = await ObterTarefaOuLancarExcecaoAsync(tarefa.Id, cancellationToken);
        return _mapeador.Map<RespostaTarefa>(tarefaCriada);
    }

    public async Task<RespostaTarefa> AtualizarAsync(
        Guid id,
        RequisicaoAtualizarTarefa requisicao,
        CancellationToken cancellationToken)
    {
        var tarefa = await ObterTarefaOuLancarExcecaoAsync(id, cancellationToken);

        NormalizarRequisicao(requisicao);
        await ValidarUsuarioAsync(requisicao.UsuarioResponsavelId, cancellationToken);

        _mapeador.Map(requisicao, tarefa);

        _repositorioTarefa.Atualizar(tarefa);
        await _unidadeTrabalho.SalvarAlteracoesAsync(cancellationToken);

        _logger.LogInformation("Tarefa {TarefaId} atualizada", tarefa.Id);

        var tarefaAtualizada = await ObterTarefaOuLancarExcecaoAsync(tarefa.Id, cancellationToken);
        return _mapeador.Map<RespostaTarefa>(tarefaAtualizada);
    }

    public async Task ExcluirAsync(Guid id, CancellationToken cancellationToken)
    {
        var tarefa = await ObterTarefaOuLancarExcecaoAsync(id, cancellationToken);

        _repositorioTarefa.Remover(tarefa);
        await _unidadeTrabalho.SalvarAlteracoesAsync(cancellationToken);

        _logger.LogInformation("Tarefa {TarefaId} excluida logicamente", tarefa.Id);
    }

    public async Task<ResultadoPaginado<RespostaComentarioTarefa>> ObterComentariosPaginadosAsync(
        Guid tarefaId,
        RequisicaoFiltroComentarioTarefa filtro,
        CancellationToken cancellationToken)
    {
        await GarantirTarefaExistenteAsync(tarefaId, cancellationToken);

        _logger.LogInformation(
            "Consultando comentarios da tarefa {TarefaId}. Pagina: {NumeroPagina}, Tamanho: {TamanhoPagina}, Conteudo: {Conteudo}, AutorId: {AutorId}",
            tarefaId,
            filtro.NumeroPagina,
            filtro.TamanhoPagina,
            filtro.Conteudo,
            filtro.AutorId);

        var comentarios = await _repositorioComentarioTarefa.ObterPaginadoPorTarefaAsync(tarefaId, filtro, cancellationToken);
        var itens = _mapeador.Map<IReadOnlyCollection<RespostaComentarioTarefa>>(comentarios.Itens);

        return new ResultadoPaginado<RespostaComentarioTarefa>(
            itens,
            comentarios.NumeroPagina,
            comentarios.TamanhoPagina,
            comentarios.TotalRegistros);
    }

    public async Task<RespostaComentarioTarefa> ObterComentarioPorIdAsync(
        Guid tarefaId,
        Guid comentarioId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Consultando comentario {ComentarioId} da tarefa {TarefaId}", comentarioId, tarefaId);

        var comentario = await ObterComentarioOuLancarExcecaoAsync(tarefaId, comentarioId, cancellationToken);
        return _mapeador.Map<RespostaComentarioTarefa>(comentario);
    }

    public async Task<RespostaComentarioTarefa> CriarComentarioAsync(
        Guid tarefaId,
        RequisicaoCriarComentarioTarefa requisicao,
        CancellationToken cancellationToken)
    {
        await GarantirTarefaExistenteAsync(tarefaId, cancellationToken);

        NormalizarRequisicao(requisicao);
        await ValidarUsuarioAsync(requisicao.AutorId, cancellationToken);

        var comentario = _mapeador.Map<TaskComment>(requisicao);
        comentario.TaskItemId = tarefaId;

        await _repositorioComentarioTarefa.AdicionarAsync(comentario, cancellationToken);
        await _unidadeTrabalho.SalvarAlteracoesAsync(cancellationToken);

        _logger.LogInformation("Comentario {ComentarioId} criado na tarefa {TarefaId}", comentario.Id, tarefaId);

        var comentarioCriado = await ObterComentarioOuLancarExcecaoAsync(tarefaId, comentario.Id, cancellationToken);
        return _mapeador.Map<RespostaComentarioTarefa>(comentarioCriado);
    }

    public async Task<RespostaComentarioTarefa> AtualizarComentarioAsync(
        Guid tarefaId,
        Guid comentarioId,
        RequisicaoAtualizarComentarioTarefa requisicao,
        CancellationToken cancellationToken)
    {
        var comentario = await ObterComentarioOuLancarExcecaoAsync(tarefaId, comentarioId, cancellationToken);

        NormalizarRequisicao(requisicao);
        await ValidarUsuarioAsync(requisicao.AutorId, cancellationToken);

        _mapeador.Map(requisicao, comentario);

        _repositorioComentarioTarefa.Atualizar(comentario);
        await _unidadeTrabalho.SalvarAlteracoesAsync(cancellationToken);

        _logger.LogInformation("Comentario {ComentarioId} da tarefa {TarefaId} atualizado", comentario.Id, tarefaId);

        var comentarioAtualizado = await ObterComentarioOuLancarExcecaoAsync(tarefaId, comentario.Id, cancellationToken);
        return _mapeador.Map<RespostaComentarioTarefa>(comentarioAtualizado);
    }

    public async Task ExcluirComentarioAsync(Guid tarefaId, Guid comentarioId, CancellationToken cancellationToken)
    {
        var comentario = await ObterComentarioOuLancarExcecaoAsync(tarefaId, comentarioId, cancellationToken);

        _repositorioComentarioTarefa.Remover(comentario);
        await _unidadeTrabalho.SalvarAlteracoesAsync(cancellationToken);

        _logger.LogInformation("Comentario {ComentarioId} da tarefa {TarefaId} excluido logicamente", comentario.Id, tarefaId);
    }

    private async Task<TaskItem> ObterTarefaOuLancarExcecaoAsync(Guid id, CancellationToken cancellationToken)
    {
        var tarefa = await _repositorioTarefa.ObterDetalhadaPorIdAsync(id, cancellationToken);
        return tarefa ?? throw new KeyNotFoundException($"Tarefa '{id}' nao foi encontrada.");
    }

    private async Task<TaskComment> ObterComentarioOuLancarExcecaoAsync(
        Guid tarefaId,
        Guid comentarioId,
        CancellationToken cancellationToken)
    {
        var comentario = await _repositorioComentarioTarefa.ObterDetalhadoPorIdAsync(tarefaId, comentarioId, cancellationToken);
        return comentario ?? throw new KeyNotFoundException($"Comentario '{comentarioId}' da tarefa '{tarefaId}' nao foi encontrado.");
    }

    private async Task GarantirTarefaExistenteAsync(Guid tarefaId, CancellationToken cancellationToken)
    {
        _ = await ObterTarefaOuLancarExcecaoAsync(tarefaId, cancellationToken);
    }

    private async Task ValidarUsuarioAsync(Guid? usuarioId, CancellationToken cancellationToken)
    {
        if (!usuarioId.HasValue)
        {
            return;
        }

        if (!await _repositorioUsuario.ExistePorIdAsync(usuarioId.Value, cancellationToken))
        {
            throw new KeyNotFoundException($"Usuario '{usuarioId}' nao foi encontrado.");
        }
    }

    private static void NormalizarRequisicao(RequisicaoCriarTarefa requisicao)
    {
        requisicao.Titulo = requisicao.Titulo.Trim();
        requisicao.Descricao = string.IsNullOrWhiteSpace(requisicao.Descricao)
            ? null
            : requisicao.Descricao.Trim();
    }

    private static void NormalizarRequisicao(RequisicaoAtualizarTarefa requisicao)
    {
        requisicao.Titulo = requisicao.Titulo.Trim();
        requisicao.Descricao = string.IsNullOrWhiteSpace(requisicao.Descricao)
            ? null
            : requisicao.Descricao.Trim();
    }

    private static void NormalizarRequisicao(RequisicaoCriarComentarioTarefa requisicao)
    {
        requisicao.Conteudo = requisicao.Conteudo.Trim();
    }

    private static void NormalizarRequisicao(RequisicaoAtualizarComentarioTarefa requisicao)
    {
        requisicao.Conteudo = requisicao.Conteudo.Trim();
    }
}