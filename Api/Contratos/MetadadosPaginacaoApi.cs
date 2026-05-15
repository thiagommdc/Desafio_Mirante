namespace DesafioMirante.Api.Contratos;

public sealed class MetadadosPaginacaoApi
{
    public int NumeroPagina { get; init; }
    public int TamanhoPagina { get; init; }
    public int TotalRegistros { get; init; }
    public int TotalPaginas { get; init; }
    public bool TemPaginaAnterior => NumeroPagina > 1;
    public bool TemProximaPagina => NumeroPagina < TotalPaginas;
}
