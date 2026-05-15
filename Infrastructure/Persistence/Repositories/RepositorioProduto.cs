using DesafioMirante.Application.Common.Models;
using DesafioMirante.Application.Features.Produtos.DTOs;
using DesafioMirante.Application.Interfaces.Persistence;
using DesafioMirante.Domain.Entities;
using DesafioMirante.Infrastructure.Persistence.Context;
using DesafioMirante.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DesafioMirante.Infrastructure.Persistence.Repositories;

public sealed class RepositorioProduto : Repositorio<Produto>, IRepositorioProduto
{
    public RepositorioProduto(ContextoAplicacao contexto)
        : base(contexto)
    {
    }

    public async Task<ResultadoPaginado<Produto>> ObterPaginadoAsync(
        RequisicaoFiltroProduto filtro,
        CancellationToken cancellationToken)
    {
        var nome = filtro.Nome?.Trim();

        var consulta = Contexto.Produtos
            .AsNoTracking()
            .TagWith("Produtos:ObterPaginado")
            .AplicarFiltroQuando(
                !string.IsNullOrWhiteSpace(nome),
                origem => origem.Where(produto => EF.Functions.Like(produto.Nome, $"%{nome}%")))
            .AplicarFiltroQuando(
                filtro.Ativo.HasValue,
                origem => origem.Where(produto => produto.Ativo == filtro.Ativo!.Value))
            .OrderBy(produto => produto.Nome)
            .ThenBy(produto => produto.Id);

        return await consulta.ParaResultadoPaginadoAsync(filtro, cancellationToken);
    }

    public async Task<bool> ExistePorSkuAsync(
        string sku,
        Guid? ignorarId,
        CancellationToken cancellationToken)
    {
        var skuNormalizado = sku.Trim().ToUpperInvariant();

        return await Contexto.Produtos.AnyAsync(
            produto => produto.Sku.ToUpper() == skuNormalizado
                && (!ignorarId.HasValue || produto.Id != ignorarId.Value),
            cancellationToken);
    }
}
