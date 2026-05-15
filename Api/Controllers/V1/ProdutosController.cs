using DesafioMirante.Api.Contratos;
using DesafioMirante.Application.Common.Models;
using DesafioMirante.Application.Features.Produtos.DTOs;
using DesafioMirante.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DesafioMirante.Api.Controllers.V1;

[ApiController]
[Route("api/v1/produtos")]
public sealed class ProdutosController : ControllerBase
{
    private readonly IServicoProduto _servicoProduto;

    public ProdutosController(IServicoProduto servicoProduto)
    {
        _servicoProduto = servicoProduto;
    }

    [HttpGet]
    [ProducesResponseType(typeof(RespostaApi<IReadOnlyCollection<RespostaProduto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RespostaApi<IReadOnlyCollection<RespostaProduto>>>> ObterTodosAsync(
        [FromQuery] RequisicaoFiltroProduto requisicao,
        CancellationToken cancellationToken)
    {
        var resultado = await _servicoProduto.ObterPaginadoAsync(requisicao, cancellationToken);
        return Ok(RespostaApiFactory.CriarPaginada(
            resultado,
            "Produtos consultados com sucesso.",
            HttpContext.TraceIdentifier));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RespostaApi<RespostaProduto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespostaApi<RespostaProduto>>> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var produto = await _servicoProduto.ObterPorIdAsync(id, cancellationToken);
        return Ok(RespostaApiFactory.CriarSucesso(
            produto,
            "Produto consultado com sucesso.",
            HttpContext.TraceIdentifier));
    }

    [HttpPost]
    [ProducesResponseType(typeof(RespostaApi<RespostaProduto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RespostaApi<RespostaProduto>>> CriarAsync(
        [FromBody] RequisicaoCriarProduto requisicao,
        CancellationToken cancellationToken)
    {
        var produto = await _servicoProduto.CriarAsync(requisicao, cancellationToken);
        var resposta = RespostaApiFactory.CriarSucesso(
            produto,
            "Produto criado com sucesso.",
            HttpContext.TraceIdentifier);

        return CreatedAtAction(nameof(ObterPorIdAsync), new { id = produto.Id }, resposta);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(RespostaApi<RespostaProduto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RespostaApi<RespostaProduto>>> AtualizarAsync(
        Guid id,
        [FromBody] RequisicaoAtualizarProduto requisicao,
        CancellationToken cancellationToken)
    {
        var produto = await _servicoProduto.AtualizarAsync(id, requisicao, cancellationToken);
        return Ok(RespostaApiFactory.CriarSucesso(
            produto,
            "Produto atualizado com sucesso.",
            HttpContext.TraceIdentifier));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(RespostaApi<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaErroApi), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespostaApi<object?>>> ExcluirAsync(Guid id, CancellationToken cancellationToken)
    {
        await _servicoProduto.ExcluirAsync(id, cancellationToken);
        return Ok(RespostaApi<object?>.CriarSucesso(
            null,
            "Produto excluido com sucesso.",
            HttpContext.TraceIdentifier));
    }
}
