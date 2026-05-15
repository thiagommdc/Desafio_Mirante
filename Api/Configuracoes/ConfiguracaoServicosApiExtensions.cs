using DesafioMirante.Api.Contratos;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace DesafioMirante.Api.Configuracoes;

public static class ConfiguracaoServicosApiExtensions
{
    public static IServiceCollection AdicionarServicosApi(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddControllers(opcoes =>
        {
            opcoes.RespectBrowserAcceptHeader = true;
            opcoes.ReturnHttpNotAcceptable = true;
        })
        .AddJsonOptions(opcoes =>
        {
            opcoes.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddFluentValidationAutoValidation();
        services.AddHealthChecks();
        services.Configure<ApiBehaviorOptions>(opcoes =>
        {
            opcoes.InvalidModelStateResponseFactory = context =>
            {
                var erros = context.ModelState
                    .Where(item => item.Value?.Errors.Count > 0)
                    .ToDictionary(
                        item => item.Key,
                        item => item.Value!.Errors.Select(erro => erro.ErrorMessage).ToArray());

                var resposta = RespostaErroApi.Criar(
                    "A requisicao possui campos invalidos.",
                    context.HttpContext.TraceIdentifier,
                    erros);

                return new BadRequestObjectResult(resposta);
            };
        });

        return services;
    }
}
