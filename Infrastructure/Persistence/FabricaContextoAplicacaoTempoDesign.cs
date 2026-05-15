using DesafioMirante.Application.Abstractions.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DesafioMirante.Infrastructure.Persistence;

public sealed class FabricaContextoAplicacaoTempoDesign : IDesignTimeDbContextFactory<ContextoAplicacao>
{
    public ContextoAplicacao CreateDbContext(string[] args)
    {
        var construtorOpcoes = new DbContextOptionsBuilder<ContextoAplicacao>();
        construtorOpcoes.UseSqlite("Data Source=desafiomirante.db");

        return new ContextoAplicacao(construtorOpcoes.Options, new ServicoUsuarioAtualTempoDesign());
    }

    private sealed class ServicoUsuarioAtualTempoDesign : IServicoUsuarioAtual
    {
        public string ObterIdentificadorUsuarioAtual()
        {
            return "migration";
        }
    }
}
