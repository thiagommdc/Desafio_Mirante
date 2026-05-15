using DesafioMirante.Application.Abstractions.Services;
using DesafioMirante.Application.Services;
using DesafioMirante.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioMirante.Application;

public static class InjecaoDependencia
{
    public static IServiceCollection AdicionarAplicacao(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(InjecaoDependencia).Assembly);
        services.AddValidatorsFromAssemblyContaining<ValidadorRequisicaoCriarProduto>();
        services.AddScoped<IServicoProduto, ServicoProduto>();

        return services;
    }
}
