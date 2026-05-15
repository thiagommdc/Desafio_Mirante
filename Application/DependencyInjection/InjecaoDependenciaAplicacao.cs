using DesafioMirante.Application.Features.Tarefas.Validators;
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
        services.AddValidatorsFromAssemblyContaining<ValidadorRequisicaoCriarTarefa>();
        services.AddScoped<IServicoTarefa, ServicoTarefa>();

        return services;
    }
}
