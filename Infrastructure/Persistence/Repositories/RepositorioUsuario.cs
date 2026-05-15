using DesafioMirante.Application.Interfaces.Persistence;
using DesafioMirante.Domain.Entities;
using DesafioMirante.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DesafioMirante.Infrastructure.Persistence.Repositories;

public sealed class RepositorioUsuario : Repositorio<User>, IRepositorioUsuario
{
    public RepositorioUsuario(ContextoAplicacao contexto)
        : base(contexto)
    {
    }

    public async Task<bool> ExistePorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Contexto.Users.AnyAsync(usuario => usuario.Id == id, cancellationToken);
    }
}