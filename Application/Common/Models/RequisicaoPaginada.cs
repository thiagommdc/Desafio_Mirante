namespace DesafioMirante.Application.Common.Models;

public abstract class RequisicaoPaginada
{
    public int NumeroPagina { get; set; } = 1;
    public int TamanhoPagina { get; set; } = 10;
}
