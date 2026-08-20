namespace Sabemi.Webhooks.Application.DTOs;

public class PagamentoWebhookResponse
{
    public bool Aceito { get; set; }
    public bool Duplicado { get; set; }
    public string Mensagem { get; set; } = default!;

    public static PagamentoWebhookResponse Aceitar() => new()
    {
        Aceito = true,
        Duplicado = false,
        Mensagem = "Evento recebido e enfileirado para processamento"
    };

    public static PagamentoWebhookResponse JaProcessado() => new()
    {
        Aceito = true,
        Duplicado = true,
        Mensagem = "Evento já processado anteriormente"
    };
}
