using DesafioMirante.Domain.Entities;

namespace DesafioMirante.Application.Interfaces.Persistence;

public interface IRepositorioUsuario : IRepositorioGenerico<User>
{
    Task<bool> ExistePorIdAsync(Guid id, CancellationToken cancellationToken);
}