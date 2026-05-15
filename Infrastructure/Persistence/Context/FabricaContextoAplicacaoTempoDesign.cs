using DesafioMirante.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DesafioMirante.Infrastructure.Persistence.Context;

public sealed class FabricaContextoAplicacaoTempoDesign : IDesignTimeDbContextFactory<ContextoAplicacao>
{
    public ContextoAplicacao CreateDbContext(string[] args)
    {
        var ambiente = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var configuracao = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Api/appsettings.json", optional: true)
            .AddJsonFile($"Api/appsettings.{ambiente}.json", optional: true)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{ambiente}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var configuracaoBancoDados = ConfiguradorProvedorBancoDados.ObterConfiguracao(configuracao);
        var construtorOpcoes = new DbContextOptionsBuilder<ContextoAplicacao>();

        ConfiguradorProvedorBancoDados.Configurar(
            construtorOpcoes,
            configuracao,
            configuracaoBancoDados);

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
