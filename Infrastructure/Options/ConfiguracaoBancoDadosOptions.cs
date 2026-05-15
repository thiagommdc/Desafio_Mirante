namespace DesafioMirante.Infrastructure.Options;

public sealed class ConfiguracaoBancoDadosOptions
{
    public const string Secao = "Persistencia";

    public string Provedor { get; set; } = "Sqlite";
    public string NomeConexaoSqlite { get; set; } = "Sqlite";
    public string NomeConexaoPostgreSql { get; set; } = "PostgreSql";
}
