namespace Sabemi.Webhooks.Application.DTOs;

public class PagamentoConsultaResponse
{
    public int Id { get; set; }
    public string IdTransacao { get; set; } = default!;
    public string IdContrato { get; set; } = default!;
    public decimal Valor { get; set; }
    public DateTime DataPagamento { get; set; }
    public string StatusRecebido { get; set; } = default!;
    public bool Processado { get; set; }
    public string? ErroProcessamento { get; set; }
    public DateTime RecebidoEm { get; set; }
}
