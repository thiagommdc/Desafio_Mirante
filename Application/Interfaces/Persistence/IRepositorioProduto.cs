using DesafioMirante.Application.Common.Models;
using DesafioMirante.Application.Features.Produtos.DTOs;
using DesafioMirante.Domain.Entities;

namespace DesafioMirante.Application.Interfaces.Persistence;

public interface IRepositorioProduto : IRepositorioGenerico<Produto>
{
    Task<ResultadoPaginado<Produto>> ObterPaginadoAsync(RequisicaoFiltroProduto filtro, CancellationToken cancellationToken);
    Task<bool> ExistePorSkuAsync(string sku, Guid? ignorarId, CancellationToken cancellationToken);
}
