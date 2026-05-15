using DesafioMirante.Application.Common.Models;

namespace DesafioMirante.Api.Contratos;

public static class RespostaApiFactory
{
    public static RespostaApi<T> CriarSucesso<T>(
        T dados,
        string mensagem,
        string traceId)
    {
        return RespostaApi<T>.CriarSucesso(dados, mensagem, traceId);
    }

    public static RespostaApi<IReadOnlyCollection<T>> CriarPaginada<T>(
        ResultadoPaginado<T> resultado,
        string mensagem,
        string traceId)
    {
        return RespostaApi<IReadOnlyCollection<T>>.CriarSucesso(
            resultado.Itens,
            mensagem,
            traceId,
            new MetadadosPaginacaoApi
            {
                NumeroPagina = resultado.NumeroPagina,
                TamanhoPagina = resultado.TamanhoPagina,
                TotalRegistros = resultado.TotalRegistros,
                TotalPaginas = resultado.TotalPaginas
            });
    }
}
