using FluentAssertions;
using Moq;
using Sabemi.Webhooks.Application.DTOs;
using Sabemi.Webhooks.Application.Interfaces;
using Sabemi.Webhooks.Application.Services;
using Sabemi.Webhooks.Domain.Entities;
using Sabemi.Webhooks.Domain.Exceptions;
using Xunit;

namespace Sabemi.Webhooks.UnitTests.Services;

public class PagamentoWebhookServiceTests
{
    private readonly Mock<IEventoBrutoRepository> _eventoBrutoRepositoryMock;
    private readonly Mock<IEventoProcessingQueue> _queueMock;
    private readonly PagamentoWebhookService _service;

    public PagamentoWebhookServiceTests()
    {
        _eventoBrutoRepositoryMock = new Mock<IEventoBrutoRepository>();
        _queueMock = new Mock<IEventoProcessingQueue>();
        _service = new PagamentoWebhookService(_eventoBrutoRepositoryMock.Object, _queueMock.Object);
    }

    private static PagamentoWebhookRequest CriarRequestValido() => new()
    {
        IdTransacao = "TXN-001",
        IdContrato = "CONTR-123",
        Valor = 1500.50m,
        DataPagamento = DateTime.UtcNow,
        Status = "Sucesso"
    };

    [Fact]
    public async Task ReceberAsync_QuandoTransacaoNaoExiste_DeveSalvarEEnfileirar()
    {
        // Arrange
        var request = CriarRequestValido();
        _eventoBrutoRepositoryMock
            .Setup(r => r.ExisteTransacaoAsync(request.IdTransacao, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var resultado = await _service.ReceberAsync(request);

        // Assert
        resultado.Aceito.Should().BeTrue();
        resultado.Duplicado.Should().BeFalse();

        _eventoBrutoRepositoryMock.Verify(
            r => r.AdicionarAsync(It.IsAny<EventoBruto>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _queueMock.Verify(
            q => q.EnfileirarAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ReceberAsync_QuandoTransacaoJaExiste_NaoDeveSalvarNemEnfileirar()
    {
        // Arrange
        var request = CriarRequestValido();
        _eventoBrutoRepositoryMock
            .Setup(r => r.ExisteTransacaoAsync(request.IdTransacao, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var resultado = await _service.ReceberAsync(request);

        // Assert
        resultado.Aceito.Should().BeTrue();
        resultado.Duplicado.Should().BeTrue();

        _eventoBrutoRepositoryMock.Verify(
            r => r.AdicionarAsync(It.IsAny<EventoBruto>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _queueMock.Verify(
            q => q.EnfileirarAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ReceberAsync_QuandoOcorreCondicaoDeCorrida_DeveRetornarComoDuplicadoSemQuebrar()
    {
        // Arrange: simula duas requisições simultâneas — a checagem em memória não pega,
        // mas o banco rejeita pela constraint UNIQUE, lançando TransacaoDuplicadaException.
        var request = CriarRequestValido();
        _eventoBrutoRepositoryMock
            .Setup(r => r.ExisteTransacaoAsync(request.IdTransacao, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _eventoBrutoRepositoryMock
            .Setup(r => r.AdicionarAsync(It.IsAny<EventoBruto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new TransacaoDuplicadaException(request.IdTransacao));

        // Act
        var resultado = await _service.ReceberAsync(request);

        // Assert
        resultado.Duplicado.Should().BeTrue();
        _queueMock.Verify(
            q => q.EnfileirarAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}