namespace Sabemi.Webhooks.Domain.Entities;

public class EventoBruto
{
    public int Id { get; private set; }
    public string IdTransacao { get; private set; } = default!;
    public string IdContrato { get; private set; } = default!;
    public decimal Valor { get; private set; }
    public DateTime DataPagamento { get; private set; }
    public string StatusRecebido { get; private set; } = default!; // status que veio no payload
    public string Payload { get; private set; } = default!;        // JSON cru para auditoria
    public DateTime RecebidoEm { get; private set; } = DateTime.UtcNow;
    public bool Processado { get; private set; }
    public string? ErroProcessamento { get; private set; }

    protected EventoBruto() { } // EF Core

    public EventoBruto(string idTransacao, string idContrato, decimal valor,
        DateTime dataPagamento, string statusRecebido, string payload)
    {
        IdTransacao = idTransacao;
        IdContrato = idContrato;
        Valor = valor;
        DataPagamento = dataPagamento;
        StatusRecebido = statusRecebido;
        Payload = payload;
        RecebidoEm = DateTime.UtcNow;
        Processado = false;
    }

    public void MarcarComoProcessado()
    {
        Processado = true;
        ErroProcessamento = null;
    }

    public void MarcarComoErro(string mensagem)
    {
        Processado = true;
        ErroProcessamento = mensagem;
    }
}