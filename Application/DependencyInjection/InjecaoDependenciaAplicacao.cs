using DesafioMirante.Application.Features.Produtos.Services;
using DesafioMirante.Application.Features.Produtos.Validators;
using DesafioMirante.Application.Features.Tarefas.Services;
using DesafioMirante.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioMirante.Application.DependencyInjection;

public static class InjecaoDependenciaAplicacao
{
    public static IServiceCollection AdicionarAplicacao(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(InjecaoDependenciaAplicacao).Assembly);
        services.AddValidatorsFromAssemblyContaining<ValidadorRequisicaoCriarProduto>();
        services.AddScoped<IServicoProduto, ServicoProduto>();
        services.AddScoped<IServicoTarefa, ServicoTarefa>();

        return services;
    }
}
