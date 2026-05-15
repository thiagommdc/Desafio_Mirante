using DesafioMirante.Application.Abstractions.Persistence;
using DesafioMirante.Application.Abstractions.Services;
using DesafioMirante.Infrastructure.Persistence;
using DesafioMirante.Infrastructure.Persistence.Repositories;
using DesafioMirante.Infrastructure.Persistence.Seed;
using DesafioMirante.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioMirante.Infrastructure;

public static class InjecaoDependencia
{
    public static IServiceCollection AdicionarInfraestrutura(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var conexao = configuration.GetConnectionString("ConexaoPadrao")
            ?? "Data Source=desafiomirante.db";

        services.AddDbContext<ContextoAplicacao>(opcoes =>
            opcoes.UseSqlite(
                conexao,
                sqlite => sqlite.MigrationsAssembly(typeof(ContextoAplicacao).Assembly.FullName)));

        services.AddScoped(typeof(IRepositorioGenerico<>), typeof(Repositorio<>));
        services.AddScoped<IRepositorioProduto, RepositorioProduto>();
        services.AddScoped<IUnidadeTrabalho, UnidadeTrabalho>();
        services.AddScoped<IServicoUsuarioAtual, ServicoUsuarioAtual>();
        services.AddScoped<IInicializadorBancoDados, InicializadorBancoDados>();

        return services;
    }
}
