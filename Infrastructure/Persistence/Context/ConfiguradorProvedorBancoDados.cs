using DesafioMirante.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DesafioMirante.Infrastructure.Persistence.Context;

public static class ConfiguradorProvedorBancoDados
{
    public static ConfiguracaoBancoDadosOptions ObterConfiguracao(IConfiguration configuration)
    {
        return configuration
            .GetSection(ConfiguracaoBancoDadosOptions.Secao)
            .Get<ConfiguracaoBancoDadosOptions>()
            ?? new ConfiguracaoBancoDadosOptions();
    }

    public static void Configurar(
        DbContextOptionsBuilder opcoes,
        IConfiguration configuration,
        ConfiguracaoBancoDadosOptions configuracaoBancoDados,
        string? assemblyMigrations = null)
    {
        var provedor = configuracaoBancoDados.Provedor.Trim().ToLowerInvariant();
        var migrationsAssembly = assemblyMigrations ?? typeof(ContextoAplicacao).Assembly.FullName;

        switch (provedor)
        {
            case "sqlite":
                var conexaoSqlite = configuration.GetConnectionString(configuracaoBancoDados.NomeConexaoSqlite)
                    ?? "Data Source=desafiomirante.db";

                opcoes.UseSqlite(
                    conexaoSqlite,
                    sqlite => sqlite.MigrationsAssembly(migrationsAssembly));
                break;

            case "postgresql":
                var conexaoPostgreSql = configuration.GetConnectionString(configuracaoBancoDados.NomeConexaoPostgreSql);

                if (string.IsNullOrWhiteSpace(conexaoPostgreSql))
                {
                    throw new InvalidOperationException("A connection string configurada para PostgreSql nao foi encontrada.");
                }

                opcoes.UseNpgsql(
                    conexaoPostgreSql,
                    npgsql => npgsql.MigrationsAssembly(migrationsAssembly));
                break;

            default:
                throw new InvalidOperationException(
                    $"O provedor de banco de dados '{configuracaoBancoDados.Provedor}' nao e suportado. Use 'Sqlite' ou 'PostgreSql'.");
        }
    }
}
