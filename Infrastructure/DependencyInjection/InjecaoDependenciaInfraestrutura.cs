using DesafioMirante.Application.Interfaces.Persistence;
using DesafioMirante.Application.Interfaces.Services;
using DesafioMirante.Infrastructure.Options;
using DesafioMirante.Infrastructure.Persistence.Context;
using DesafioMirante.Infrastructure.Persistence.Repositories;
using DesafioMirante.Infrastructure.Persistence.Seed;
using DesafioMirante.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioMirante.Infrastructure.DependencyInjection;

public static class InjecaoDependenciaInfraestrutura
{
    public static IServiceCollection AdicionarInfraestrutura(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var configuracaoBancoDados = ConfiguradorProvedorBancoDados.ObterConfiguracao(configuration);

        services.AddDbContext<ContextoAplicacao>(opcoes =>
            ConfiguradorProvedorBancoDados.Configurar(opcoes, configuration, configuracaoBancoDados));

        services.AddScoped(typeof(IRepositorioGenerico<>), typeof(Repositorio<>));
        services.AddScoped<IRepositorioProduto, RepositorioProduto>();
        services.AddScoped<IRepositorioTarefa, RepositorioTarefa>();
        services.AddScoped<IRepositorioComentarioTarefa, RepositorioComentarioTarefa>();
        services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
        services.AddScoped<IUnidadeTrabalho, UnidadeTrabalho>();
        services.AddScoped<IServicoUsuarioAtual, ServicoUsuarioAtual>();
        services.AddScoped<IInicializadorBancoDados, InicializadorBancoDados>();

        return services;
    }
}
