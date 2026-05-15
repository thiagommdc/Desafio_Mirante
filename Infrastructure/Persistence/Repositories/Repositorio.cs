using DesafioMirante.Application.Abstractions.Persistence;
using DesafioMirante.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace DesafioMirante.Infrastructure.Persistence.Repositories;

public class Repositorio<TEntidade> : IRepositorioGenerico<TEntidade>
    where TEntidade : EntidadeAuditavelBase
{
    protected readonly ContextoAplicacao Contexto;
    protected readonly DbSet<TEntidade> Conjunto;

    public Repositorio(ContextoAplicacao contexto)
    {
        Contexto = contexto;
        Conjunto = contexto.Set<TEntidade>();
    }

    public virtual async Task<TEntidade?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Conjunto.FirstOrDefaultAsync(entidade => entidade.Id == id, cancellationToken);
    }

    public virtual async Task AdicionarAsync(TEntidade entidade, CancellationToken cancellationToken)
    {
        await Conjunto.AddAsync(entidade, cancellationToken);
    }

    public virtual void Atualizar(TEntidade entidade)
    {
        Conjunto.Update(entidade);
    }

    public virtual void Remover(TEntidade entidade)
    {
        Conjunto.Remove(entidade);
    }
}
