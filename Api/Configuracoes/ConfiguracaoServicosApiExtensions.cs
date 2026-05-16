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
                var erros = RespostaErroApiFactory.CriarErrosValidacao(context.ModelState);
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILogger<Program>>();

                logger.LogWarning(
                    "Requisicao HTTP invalida. Metodo: {Metodo}, Caminho: {Caminho}, StatusCode: {StatusCode}, TraceId: {TraceId}, CamposInvalidos: {CamposInvalidos}",
                    context.HttpContext.Request.Method,
                    context.HttpContext.Request.Path,
                    StatusCodes.Status400BadRequest,
                    context.HttpContext.TraceIdentifier,
                    erros.Keys);

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
