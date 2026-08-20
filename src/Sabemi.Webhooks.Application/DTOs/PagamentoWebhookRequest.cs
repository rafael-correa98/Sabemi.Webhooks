using System.ComponentModel.DataAnnotations;

namespace Sabemi.Webhooks.Application.DTOs;

public class PagamentoWebhookRequest
{
    [Required]
    public string IdTransacao { get; set; } = default!;

    [Required]
    public string IdContrato { get; set; } = default!;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
    public decimal Valor { get; set; }

    [Required]
    public DateTime DataPagamento { get; set; }

    [Required]
    public string Status { get; set; } = default!;
}
