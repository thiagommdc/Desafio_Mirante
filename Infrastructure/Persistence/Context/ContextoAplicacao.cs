using DesafioMirante.Application.Interfaces.Services;
using DesafioMirante.Domain.Common;
using DesafioMirante.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioMirante.Infrastructure.Persistence.Context;

public sealed class ContextoAplicacao : DbContext
{
    private readonly IServicoUsuarioAtual _servicoUsuarioAtual;

    public ContextoAplicacao(
        DbContextOptions<ContextoAplicacao> options,
        IServicoUsuarioAtual servicoUsuarioAtual)
        : base(options)
    {
        _servicoUsuarioAtual = servicoUsuarioAtual;
    }

    public DbSet<Produto> Produtos => Set<Produto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContextoAplicacao).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AplicarInformacoesAuditoria();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void AplicarInformacoesAuditoria()
    {
        var usuarioAtual = _servicoUsuarioAtual.ObterIdentificadorUsuarioAtual();
        var agoraUtc = DateTime.UtcNow;

        foreach (var entrada in ChangeTracker.Entries<EntidadeAuditavelBase>())
        {
            switch (entrada.State)
            {
                case EntityState.Added:
                    entrada.Entity.CriadoEmUtc = agoraUtc;
                    entrada.Entity.CriadoPor = usuarioAtual;
                    entrada.Entity.Excluido = false;
                    break;

                case EntityState.Modified:
                    entrada.Entity.AtualizadoEmUtc = agoraUtc;
                    entrada.Entity.AtualizadoPor = usuarioAtual;
                    break;

                case EntityState.Deleted:
                    entrada.State = EntityState.Modified;
                    entrada.Entity.Excluido = true;
                    entrada.Entity.ExcluidoEmUtc = agoraUtc;
                    entrada.Entity.ExcluidoPor = usuarioAtual;
                    entrada.Entity.AtualizadoEmUtc = agoraUtc;
                    entrada.Entity.AtualizadoPor = usuarioAtual;
                    break;
            }
        }
    }
}
