namespace Sabemi.Webhooks.Application.DTOs;

public class StatusContratoResponse
{
    public string IdContrato { get; set; } = default!;
    public string UltimoIdTransacao { get; set; } = default!;
    public string StatusAtual { get; set; } = default!;
    public decimal ValorPago { get; set; }
    public DateTime DataUltimoPagamento { get; set; }
    public DateTime AtualizadoEm { get; set; }
}