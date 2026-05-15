namespace DesafioMirante.Application.Interfaces.Persistence;

public interface IUnidadeTrabalho
{
    Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken);
}
