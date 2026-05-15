using DesafioMirante.Domain.Common;

namespace DesafioMirante.Application.Abstractions.Persistence;

public interface IRepositorioGenerico<TEntidade>
    where TEntidade : EntidadeAuditavelBase
{
    Task<TEntidade?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task AdicionarAsync(TEntidade entidade, CancellationToken cancellationToken);
    void Atualizar(TEntidade entidade);
    void Remover(TEntidade entidade);
}
