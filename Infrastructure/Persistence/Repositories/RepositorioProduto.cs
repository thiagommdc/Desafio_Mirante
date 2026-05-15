using DesafioMirante.Application.Abstractions.Persistence;
using DesafioMirante.Application.Common.Models;
using DesafioMirante.Application.DTOs.Products;
using DesafioMirante.Domain.Entities;
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
        var consulta = Contexto.Produtos.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro.TermoBusca))
        {
            var termoBusca = filtro.TermoBusca.Trim();

            consulta = consulta.Where(produto =>
                EF.Functions.Like(produto.Nome, $"%{termoBusca}%")
                || EF.Functions.Like(produto.Sku, $"%{termoBusca}%"));
        }

        if (filtro.PrecoMinimo.HasValue)
        {
            consulta = consulta.Where(produto => produto.Preco >= filtro.PrecoMinimo.Value);
        }

        if (filtro.PrecoMaximo.HasValue)
        {
            consulta = consulta.Where(produto => produto.Preco <= filtro.PrecoMaximo.Value);
        }

        if (filtro.Ativo.HasValue)
        {
            consulta = consulta.Where(produto => produto.Ativo == filtro.Ativo.Value);
        }

        var totalRegistros = await consulta.CountAsync(cancellationToken);

        var itens = await consulta
            .OrderBy(produto => produto.Nome)
            .Skip((filtro.NumeroPagina - 1) * filtro.TamanhoPagina)
            .Take(filtro.TamanhoPagina)
            .ToListAsync(cancellationToken);

        return new ResultadoPaginado<Produto>(
            itens,
            filtro.NumeroPagina,
            filtro.TamanhoPagina,
            totalRegistros);
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
