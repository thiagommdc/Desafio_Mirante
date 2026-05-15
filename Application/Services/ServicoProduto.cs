using AutoMapper;
using DesafioMirante.Application.Abstractions.Persistence;
using DesafioMirante.Application.Abstractions.Services;
using DesafioMirante.Application.Common.Models;
using DesafioMirante.Application.DTOs.Products;
using DesafioMirante.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DesafioMirante.Application.Services;

public sealed class ServicoProduto : IServicoProduto
{
    private readonly IRepositorioProduto _repositorioProduto;
    private readonly IUnidadeTrabalho _unidadeTrabalho;
    private readonly IMapper _mapeador;
    private readonly ILogger<ServicoProduto> _logger;

    public ServicoProduto(
        IRepositorioProduto repositorioProduto,
        IUnidadeTrabalho unidadeTrabalho,
        IMapper mapeador,
        ILogger<ServicoProduto> logger)
    {
        _repositorioProduto = repositorioProduto;
        _unidadeTrabalho = unidadeTrabalho;
        _mapeador = mapeador;
        _logger = logger;
    }

    public async Task<ResultadoPaginado<RespostaProduto>> ObterPaginadoAsync(
        RequisicaoFiltroProduto filtro,
        CancellationToken cancellationToken)
    {
        var produtos = await _repositorioProduto.ObterPaginadoAsync(filtro, cancellationToken);
        var itens = _mapeador.Map<IReadOnlyCollection<RespostaProduto>>(produtos.Itens);

        return new ResultadoPaginado<RespostaProduto>(
            itens,
            produtos.NumeroPagina,
            produtos.TamanhoPagina,
            produtos.TotalRegistros);
    }

    public async Task<RespostaProduto> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var produto = await ObterProdutoOuLancarExcecaoAsync(id, cancellationToken);
        return _mapeador.Map<RespostaProduto>(produto);
    }

    public async Task<RespostaProduto> CriarAsync(
        RequisicaoCriarProduto requisicao,
        CancellationToken cancellationToken)
    {
        if (await _repositorioProduto.ExistePorSkuAsync(requisicao.Sku, null, cancellationToken))
        {
            throw new InvalidOperationException($"Ja existe um produto com o SKU '{requisicao.Sku}'.");
        }

        var produto = _mapeador.Map<Produto>(requisicao);

        await _repositorioProduto.AdicionarAsync(produto, cancellationToken);
        await _unidadeTrabalho.SalvarAlteracoesAsync(cancellationToken);

        _logger.LogInformation("Produto {ProdutoId} criado com SKU {Sku}", produto.Id, produto.Sku);

        return _mapeador.Map<RespostaProduto>(produto);
    }

    public async Task<RespostaProduto> AtualizarAsync(
        Guid id,
        RequisicaoAtualizarProduto requisicao,
        CancellationToken cancellationToken)
    {
        var produto = await ObterProdutoOuLancarExcecaoAsync(id, cancellationToken);

        if (await _repositorioProduto.ExistePorSkuAsync(requisicao.Sku, id, cancellationToken))
        {
            throw new InvalidOperationException($"Ja existe um produto com o SKU '{requisicao.Sku}'.");
        }

        _mapeador.Map(requisicao, produto);

        _repositorioProduto.Atualizar(produto);
        await _unidadeTrabalho.SalvarAlteracoesAsync(cancellationToken);

        _logger.LogInformation("Produto {ProdutoId} atualizado", produto.Id);

        return _mapeador.Map<RespostaProduto>(produto);
    }

    public async Task ExcluirAsync(Guid id, CancellationToken cancellationToken)
    {
        var produto = await ObterProdutoOuLancarExcecaoAsync(id, cancellationToken);

        _repositorioProduto.Remover(produto);
        await _unidadeTrabalho.SalvarAlteracoesAsync(cancellationToken);

        _logger.LogInformation("Produto {ProdutoId} excluido logicamente", produto.Id);
    }

    private async Task<Produto> ObterProdutoOuLancarExcecaoAsync(Guid id, CancellationToken cancellationToken)
    {
        var produto = await _repositorioProduto.ObterPorIdAsync(id, cancellationToken);

        return produto ?? throw new KeyNotFoundException($"Produto '{id}' nao foi encontrado.");
    }
}
