namespace DesafioMirante.Application.Abstractions.Persistence;

public interface IUnidadeTrabalho
{
    Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken);
}
