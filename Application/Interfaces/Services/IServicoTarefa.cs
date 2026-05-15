using DesafioMirante.Application.Common.Models;
using DesafioMirante.Application.Features.Tarefas.DTOs;

namespace DesafioMirante.Application.Interfaces.Services;

public interface IServicoTarefa
{
    Task<ResultadoPaginado<RespostaTarefa>> ObterPaginadoAsync(RequisicaoFiltroTarefa filtro, CancellationToken cancellationToken);
    Task<RespostaTarefa> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<RespostaTarefa> CriarAsync(RequisicaoCriarTarefa requisicao, CancellationToken cancellationToken);
    Task<RespostaTarefa> AtualizarAsync(Guid id, RequisicaoAtualizarTarefa requisicao, CancellationToken cancellationToken);
    Task ExcluirAsync(Guid id, CancellationToken cancellationToken);
    Task<ResultadoPaginado<RespostaComentarioTarefa>> ObterComentariosPaginadosAsync(Guid tarefaId, RequisicaoFiltroComentarioTarefa filtro, CancellationToken cancellationToken);
    Task<RespostaComentarioTarefa> ObterComentarioPorIdAsync(Guid tarefaId, Guid comentarioId, CancellationToken cancellationToken);
    Task<RespostaComentarioTarefa> CriarComentarioAsync(Guid tarefaId, RequisicaoCriarComentarioTarefa requisicao, CancellationToken cancellationToken);
    Task<RespostaComentarioTarefa> AtualizarComentarioAsync(Guid tarefaId, Guid comentarioId, RequisicaoAtualizarComentarioTarefa requisicao, CancellationToken cancellationToken);
    Task ExcluirComentarioAsync(Guid tarefaId, Guid comentarioId, CancellationToken cancellationToken);
}