namespace DesafioMirante.Infrastructure.Services;

public interface IInicializadorBancoDados
{
    Task InicializarAsync(CancellationToken cancellationToken);
}
