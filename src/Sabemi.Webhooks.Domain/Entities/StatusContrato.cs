namespace Sabemi.Webhooks.Domain.Entities;

public class StatusContrato
{
    public string IdContrato { get; private set; } = default!; // PK
    public string UltimoIdTransacao { get; private set; } = default!;
    public string StatusAtual { get; private set; } = default!; // Sucesso / Erro / Pendente
    public decimal ValorPago { get; private set; }
    public DateTime DataUltimoPagamento { get; private set; }
    public DateTime AtualizadoEm { get; private set; } = DateTime.UtcNow;

    protected StatusContrato() { } // EF Core

    public StatusContrato(string idContrato, string idTransacao, string statusAtual,
        decimal valorPago, DateTime dataPagamento)
    {
        IdContrato = idContrato;
        UltimoIdTransacao = idTransacao;
        StatusAtual = statusAtual;
        ValorPago = valorPago;
        DataUltimoPagamento = dataPagamento;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void AtualizarStatus(string idTransacao, string statusAtual, decimal valorPago, DateTime dataPagamento)
    {
        UltimoIdTransacao = idTransacao;
        StatusAtual = statusAtual;
        ValorPago = valorPago;
        DataUltimoPagamento = dataPagamento;
        AtualizadoEm = DateTime.UtcNow;
    }
}