using DesafioMirante.Api.Configuracoes;
using DesafioMirante.Api.Middlewares;
using DesafioMirante.Application.DependencyInjection;
using DesafioMirante.Infrastructure.DependencyInjection;
using DesafioMirante.Infrastructure.Services;

var construtor = WebApplication.CreateBuilder(args);

construtor.AdicionarConfiguracaoLogging();
construtor.Services.AdicionarServicosApi();
construtor.Services.AdicionarAplicacao();
construtor.Services.AdicionarInfraestrutura(construtor.Configuration);

var aplicacao = construtor.Build();

aplicacao.UseMiddleware<MiddlewareCorrelacaoRequisicao>();
aplicacao.UseMiddleware<MiddlewareCabecalhosSeguranca>();
aplicacao.UseMiddleware<MiddlewareLoggingRequisicao>();
aplicacao.UseMiddleware<MiddlewareTratamentoExcecoes>();

if (aplicacao.Environment.IsDevelopment())
{
    aplicacao.UseSwagger();
    aplicacao.UseSwaggerUI();
}
else
{
    aplicacao.UseHsts();
}

aplicacao.UseHttpsRedirection();
aplicacao.MapHealthChecks("/health");
aplicacao.MapControllers();

using (var escopo = aplicacao.Services.CreateScope())
{
    var inicializadorBancoDados = escopo.ServiceProvider.GetRequiredService<IInicializadorBancoDados>();
    await inicializadorBancoDados.InicializarAsync(aplicacao.Lifetime.ApplicationStopping);
}

await aplicacao.RunAsync();
