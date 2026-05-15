using DesafioMirante.Api.Contratos;
using DesafioMirante.Application.Common.Models;
using DesafioMirante.Application.Features.Tarefas.DTOs;
using DesafioMirante.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DesafioMirante.Api.Controllers.V1;

[ApiController]
[Route("api/v1/tarefas")]
public sealed class TarefasController : ControllerBase
{
    private readonly IServicoTarefa _servicoTarefa;

    public TarefasController(IServicoTarefa servicoTarefa)
    {
        _servicoTarefa = servicoTarefa;
    }

    [HttpGet]
    [ProducesResponseType(typeof(RespostaApi<IReadOnlyCollection<RespostaTarefa>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RespostaApi<IReadOnlyCollection<RespostaTarefa>>>> ObterTodosAsync(
        [FromQuery] RequisicaoFiltroTarefa requisicao,
        CancellationToken cancellationToken)
    {
        var resultado = await _servicoTarefa.ObterPaginadoAsync(requisicao, cancellationToken);
        return Ok(RespostaApiFactory.CriarPaginada(
            resultado,
            "Tarefas consultadas com sucesso.",
            HttpContext.TraceIdentifier));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RespostaApi<RespostaTarefa>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespostaApi<RespostaTarefa>>> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var tarefa = await _servicoTarefa.ObterPorIdAsync(id, cancellationToken);
        return Ok(RespostaApiFactory.CriarSucesso(
            tarefa,
            "Tarefa consultada com sucesso.",
            HttpContext.TraceIdentifier));
    }

    [HttpPost]
    [ProducesResponseType(typeof(RespostaApi<RespostaTarefa>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespostaApi<RespostaTarefa>>> CriarAsync(
        [FromBody] RequisicaoCriarTarefa requisicao,
        CancellationToken cancellationToken)
    {
        var tarefa = await _servicoTarefa.CriarAsync(requisicao, cancellationToken);
        var resposta = RespostaApiFactory.CriarSucesso(
            tarefa,
            "Tarefa criada com sucesso.",
            HttpContext.TraceIdentifier);

        return CreatedAtAction(nameof(ObterPorIdAsync), new { id = tarefa.Id }, resposta);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(RespostaApi<RespostaTarefa>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespostaApi<RespostaTarefa>>> AtualizarAsync(
        Guid id,
        [FromBody] RequisicaoAtualizarTarefa requisicao,
        CancellationToken cancellationToken)
    {
        var tarefa = await _servicoTarefa.AtualizarAsync(id, requisicao, cancellationToken);
        return Ok(RespostaApiFactory.CriarSucesso(
            tarefa,
            "Tarefa atualizada com sucesso.",
            HttpContext.TraceIdentifier));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(RespostaApi<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespostaApi<object?>>> ExcluirAsync(Guid id, CancellationToken cancellationToken)
    {
        await _servicoTarefa.ExcluirAsync(id, cancellationToken);
        return Ok(RespostaApi<object?>.CriarSucesso(
            null,
            "Tarefa excluida com sucesso.",
            HttpContext.TraceIdentifier));
    }

    [HttpGet("{tarefaId:guid}/comentarios")]
    [ProducesResponseType(typeof(RespostaApi<IReadOnlyCollection<RespostaComentarioTarefa>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespostaApi<IReadOnlyCollection<RespostaComentarioTarefa>>>> ObterComentariosAsync(
        Guid tarefaId,
        [FromQuery] RequisicaoFiltroComentarioTarefa requisicao,
        CancellationToken cancellationToken)
    {
        var resultado = await _servicoTarefa.ObterComentariosPaginadosAsync(tarefaId, requisicao, cancellationToken);
        return Ok(RespostaApiFactory.CriarPaginada(
            resultado,
            "Comentarios consultados com sucesso.",
            HttpContext.TraceIdentifier));
    }

    [HttpGet("{tarefaId:guid}/comentarios/{comentarioId:guid}")]
    [ProducesResponseType(typeof(RespostaApi<RespostaComentarioTarefa>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespostaApi<RespostaComentarioTarefa>>> ObterComentarioPorIdAsync(
        Guid tarefaId,
        Guid comentarioId,
        CancellationToken cancellationToken)
    {
        var comentario = await _servicoTarefa.ObterComentarioPorIdAsync(tarefaId, comentarioId, cancellationToken);
        return Ok(RespostaApiFactory.CriarSucesso(
            comentario,
            "Comentario consultado com sucesso.",
            HttpContext.TraceIdentifier));
    }

    [HttpPost("{tarefaId:guid}/comentarios")]
    [ProducesResponseType(typeof(RespostaApi<RespostaComentarioTarefa>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespostaApi<RespostaComentarioTarefa>>> CriarComentarioAsync(
        Guid tarefaId,
        [FromBody] RequisicaoCriarComentarioTarefa requisicao,
        CancellationToken cancellationToken)
    {
        var comentario = await _servicoTarefa.CriarComentarioAsync(tarefaId, requisicao, cancellationToken);
        var resposta = RespostaApiFactory.CriarSucesso(
            comentario,
            "Comentario criado com sucesso.",
            HttpContext.TraceIdentifier);

        return CreatedAtAction(
            nameof(ObterComentarioPorIdAsync),
            new { tarefaId, comentarioId = comentario.Id },
            resposta);
    }

    [HttpPut("{tarefaId:guid}/comentarios/{comentarioId:guid}")]
    [ProducesResponseType(typeof(RespostaApi<RespostaComentarioTarefa>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespostaApi<RespostaComentarioTarefa>>> AtualizarComentarioAsync(
        Guid tarefaId,
        Guid comentarioId,
        [FromBody] RequisicaoAtualizarComentarioTarefa requisicao,
        CancellationToken cancellationToken)
    {
        var comentario = await _servicoTarefa.AtualizarComentarioAsync(tarefaId, comentarioId, requisicao, cancellationToken);
        return Ok(RespostaApiFactory.CriarSucesso(
            comentario,
            "Comentario atualizado com sucesso.",
            HttpContext.TraceIdentifier));
    }

    [HttpDelete("{tarefaId:guid}/comentarios/{comentarioId:guid}")]
    [ProducesResponseType(typeof(RespostaApi<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespostaApi<object?>>> ExcluirComentarioAsync(
        Guid tarefaId,
        Guid comentarioId,
        CancellationToken cancellationToken)
    {
        await _servicoTarefa.ExcluirComentarioAsync(tarefaId, comentarioId, cancellationToken);
        return Ok(RespostaApi<object?>.CriarSucesso(
            null,
            "Comentario excluido com sucesso.",
            HttpContext.TraceIdentifier));
    }
}