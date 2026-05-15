using DesafioMirante.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioMirante.Infrastructure.Persistence.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> AplicarFiltroQuando<T>(
        this IQueryable<T> consulta,
        bool condicao,
        Func<IQueryable<T>, IQueryable<T>> aplicarFiltro)
    {
        return condicao ? aplicarFiltro(consulta) : consulta;
    }

    public static IQueryable<T> AplicarPaginacao<T>(
        this IQueryable<T> consulta,
        RequisicaoPaginada requisicao)
    {
        return consulta
            .Skip((requisicao.NumeroPagina - 1) * requisicao.TamanhoPagina)
            .Take(requisicao.TamanhoPagina);
    }

    public static async Task<ResultadoPaginado<T>> ParaResultadoPaginadoAsync<T>(
        this IQueryable<T> consulta,
        RequisicaoPaginada requisicao,
        CancellationToken cancellationToken)
    {
        var totalRegistros = await consulta.CountAsync(cancellationToken);

        var itens = await consulta
            .AplicarPaginacao(requisicao)
            .ToListAsync(cancellationToken);

        return new ResultadoPaginado<T>(
            itens,
            requisicao.NumeroPagina,
            requisicao.TamanhoPagina,
            totalRegistros);
    }
}
