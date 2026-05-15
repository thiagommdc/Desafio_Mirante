using DesafioMirante.Application.Abstractions.Services;
using DesafioMirante.Application.Common.Models;
using DesafioMirante.Application.DTOs.Products;
using Microsoft.AspNetCore.Mvc;

namespace DesafioMirante.Api.Controllers;

[ApiController]
[Route("api/produtos")]
public sealed class ProdutosController : ControllerBase
{
    private readonly IServicoProduto _servicoProduto;

    public ProdutosController(IServicoProduto servicoProduto)
    {
        _servicoProduto = servicoProduto;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResultadoPaginado<RespostaProduto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ResultadoPaginado<RespostaProduto>>> ObterTodosAsync(
        [FromQuery] RequisicaoFiltroProduto requisicao,
        CancellationToken cancellationToken)
    {
        var resultado = await _servicoProduto.ObterPaginadoAsync(requisicao, cancellationToken);
        return Ok(resultado);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RespostaProduto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespostaProduto>> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var produto = await _servicoProduto.ObterPorIdAsync(id, cancellationToken);
        return Ok(produto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(RespostaProduto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RespostaProduto>> CriarAsync(
        [FromBody] RequisicaoCriarProduto requisicao,
        CancellationToken cancellationToken)
    {
        var produto = await _servicoProduto.CriarAsync(requisicao, cancellationToken);
        return CreatedAtAction(nameof(ObterPorIdAsync), new { id = produto.Id }, produto);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(RespostaProduto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RespostaProduto>> AtualizarAsync(
        Guid id,
        [FromBody] RequisicaoAtualizarProduto requisicao,
        CancellationToken cancellationToken)
    {
        var produto = await _servicoProduto.AtualizarAsync(id, requisicao, cancellationToken);
        return Ok(produto);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExcluirAsync(Guid id, CancellationToken cancellationToken)
    {
        await _servicoProduto.ExcluirAsync(id, cancellationToken);
        return NoContent();
    }
}
