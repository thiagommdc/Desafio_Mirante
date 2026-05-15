using DesafioMirante.Application.Abstractions.Persistence;

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
