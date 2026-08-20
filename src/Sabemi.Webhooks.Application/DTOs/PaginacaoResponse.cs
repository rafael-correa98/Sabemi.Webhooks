namespace Sabemi.Webhooks.Application.DTOs;

public class PaginacaoResponse<T>
{
    public IEnumerable<T> Itens { get; set; } = Enumerable.Empty<T>();
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
    public int TotalPaginas => TamanhoPagina == 0 ? 0 : (int)Math.Ceiling((double)Total / TamanhoPagina);
}