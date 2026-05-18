using System.Text.Json;
using DesafioMirante.Api.Contratos;
using DesafioMirante.Api.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace DesafioMirante.UnitTests.Api;

public sealed class MiddlewareTratamentoExcecoesTests
{
    [Fact]
    public async Task InvokeAsync_DeveRetornar404QuandoOcorrerKeyNotFoundException()
    {
        var contexto = new DefaultHttpContext();
        contexto.TraceIdentifier = "trace-teste";
        contexto.Request.Method = "GET";
        contexto.Request.Path = "/api/v1/tarefas/123";
        contexto.Response.Body = new MemoryStream();

        RequestDelegate proximo = _ => throw new KeyNotFoundException("Tarefa nao encontrada.");
        var middleware = new MiddlewareTratamentoExcecoes(
            proximo,
            NullLogger<MiddlewareTratamentoExcecoes>.Instance);

        await middleware.InvokeAsync(contexto);

        contexto.Response.Body.Position = 0;
        var resposta = await JsonSerializer.DeserializeAsync<RespostaErroApi>(
            contexto.Response.Body,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        Assert.NotNull(resposta);
        Assert.Equal(StatusCodes.Status404NotFound, contexto.Response.StatusCode);
        Assert.StartsWith("application/json", contexto.Response.ContentType);
        Assert.False(resposta.Sucesso);
        Assert.Equal("Tarefa nao encontrada.", resposta.Mensagem);
        Assert.Equal("trace-teste", resposta.TraceId);
    }
}
