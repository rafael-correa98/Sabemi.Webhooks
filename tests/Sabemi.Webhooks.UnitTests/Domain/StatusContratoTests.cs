using FluentAssertions;
using Sabemi.Webhooks.Domain.Entities;
using Xunit;

namespace Sabemi.Webhooks.UnitTests.Domain;

public class StatusContratoTests
{
    [Fact]
    public void AtualizarStatus_DeveSobrescreverDadosComOsMaisRecentes()
    {
        // Arrange
        var status = new StatusContrato("CONTR-123", "TXN-001", "Sucesso", 1000m, DateTime.UtcNow.AddDays(-1));
        var novaData = DateTime.UtcNow;

        // Act
        status.AtualizarStatus("TXN-002", "Erro", 500m, novaData);

        // Assert
        status.UltimoIdTransacao.Should().Be("TXN-002");
        status.StatusAtual.Should().Be("Erro");
        status.ValorPago.Should().Be(500m);
        status.DataUltimoPagamento.Should().Be(novaData);
    }

    [Fact]
    public void MarcarComoProcessado_DeveLimparErroAnterior()
    {
        var evento = new EventoBruto("TXN-001", "CONTR-123", 1000m, DateTime.UtcNow, "Sucesso", "{}");
        evento.MarcarComoErro("Falha temporária");

        evento.MarcarComoProcessado();

        evento.Processado.Should().BeTrue();
        evento.ErroProcessamento.Should().BeNull();
    }
}