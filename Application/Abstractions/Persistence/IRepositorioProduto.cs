using DesafioMirante.Application.Common.Models;
using DesafioMirante.Application.DTOs.Products;
using DesafioMirante.Domain.Entities;

namespace DesafioMirante.Application.Abstractions.Persistence;

public interface IRepositorioProduto : IRepositorioGenerico<Produto>
{
    Task<ResultadoPaginado<Produto>> ObterPaginadoAsync(RequisicaoFiltroProduto filtro, CancellationToken cancellationToken);
    Task<bool> ExistePorSkuAsync(string sku, Guid? ignorarId, CancellationToken cancellationToken);
}
