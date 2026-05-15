namespace DesafioMirante.Application.Common.Models;

public sealed class ResultadoPaginado<T>
{
    public ResultadoPaginado(
        IReadOnlyCollection<T> itens,
        int numeroPagina,
        int tamanhoPagina,
        int totalRegistros)
    {
        Itens = itens;
        NumeroPagina = numeroPagina;
        TamanhoPagina = tamanhoPagina;
        TotalRegistros = totalRegistros;
        TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanhoPagina);
    }

    public IReadOnlyCollection<T> Itens { get; }
    public int NumeroPagina { get; }
    public int TamanhoPagina { get; }
    public int TotalRegistros { get; }
    public int TotalPaginas { get; }
}
