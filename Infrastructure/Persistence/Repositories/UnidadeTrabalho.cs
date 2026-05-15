using DesafioMirante.Application.Interfaces.Persistence;
using DesafioMirante.Infrastructure.Persistence.Context;

namespace DesafioMirante.Infrastructure.Persistence.Repositories;

public sealed class UnidadeTrabalho : IUnidadeTrabalho
{
    private readonly ContextoAplicacao _contexto;

    public UnidadeTrabalho(ContextoAplicacao contexto)
    {
        _contexto = contexto;
    }

    public Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken)
    {
        return _contexto.SaveChangesAsync(cancellationToken);
    }
}
