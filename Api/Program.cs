using DesafioMirante.Api.Middleware;
using DesafioMirante.Application;
using DesafioMirante.Infrastructure;
using DesafioMirante.Infrastructure.Services;
using FluentValidation.AspNetCore;

var construtor = WebApplication.CreateBuilder(args);

construtor.Logging.ClearProviders();
construtor.Logging.AddConsole();
construtor.Logging.AddDebug();

construtor.Services.AddHttpContextAccessor();
construtor.Services.AddControllers();
construtor.Services.AddEndpointsApiExplorer();
construtor.Services.AddSwaggerGen();
construtor.Services.AddFluentValidationAutoValidation();
construtor.Services.AdicionarAplicacao();
construtor.Services.AdicionarInfraestrutura(construtor.Configuration);

var aplicacao = construtor.Build();

aplicacao.UseMiddleware<MiddlewareTratamentoExcecoes>();

if (aplicacao.Environment.IsDevelopment())
{
    aplicacao.UseSwagger();
    aplicacao.UseSwaggerUI();
}

aplicacao.UseHttpsRedirection();
aplicacao.MapControllers();

using (var escopo = aplicacao.Services.CreateScope())
{
    var inicializadorBancoDados = escopo.ServiceProvider.GetRequiredService<IInicializadorBancoDados>();
    await inicializadorBancoDados.InicializarAsync(aplicacao.Lifetime.ApplicationStopping);
}

await aplicacao.RunAsync();
