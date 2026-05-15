using DesafioMirante.Application.Common.Models;
using DesafioMirante.Application.Features.Produtos.DTOs;

namespace DesafioMirante.Application.Interfaces.Services;

public interface IServicoProduto
{
    Task<ResultadoPaginado<RespostaProduto>> ObterPaginadoAsync(RequisicaoFiltroProduto filtro, CancellationToken cancellationToken);
    Task<RespostaProduto> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<RespostaProduto> CriarAsync(RequisicaoCriarProduto requisicao, CancellationToken cancellationToken);
    Task<RespostaProduto> AtualizarAsync(Guid id, RequisicaoAtualizarProduto requisicao, CancellationToken cancellationToken);
    Task ExcluirAsync(Guid id, CancellationToken cancellationToken);
}
