namespace Sabemi.Webhooks.Domain.Exceptions;

public class TransacaoDuplicadaException : Exception
{
    public string IdTransacao { get; }

    public TransacaoDuplicadaException(string idTransacao)
        : base($"A transação '{idTransacao}' já foi processada anteriormente.")
    {
        IdTransacao = idTransacao;
    }
}
