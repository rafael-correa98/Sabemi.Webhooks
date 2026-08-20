import StatusBadge from "./StatusBadge";
import { formatarMoeda, formatarData, derivarSituacao } from "../utils/format";

const ACCENT_POR_SITUACAO = {
  "erro-processamento": "row--erro",
  pendente: "row--pendente",
  sucesso: "row--sucesso",
  neutro: "row--neutro",
};

export default function PaymentsTable({ eventos, carregando, erro, onSelecionarContrato }) {
  if (erro) {
    return (
      <div className="empty-state empty-state--erro">
        <strong>Não foi possível carregar os pagamentos.</strong>
        <span>{erro}</span>
      </div>
    );
  }

  if (!carregando && eventos.length === 0) {
    return (
      <div className="empty-state">
        <strong>Nenhum pagamento encontrado.</strong>
        <span>Ajuste os filtros ou aguarde a chegada de novas notificações do banco.</span>
      </div>
    );
  }

  return (
    <div className="table-scroll">
      <table className="ledger-table">
        <thead>
          <tr>
            <th aria-hidden="true"></th>
            <th>Transação</th>
            <th>Contrato</th>
            <th>Valor</th>
            <th>Data do pagamento</th>
            <th>Status</th>
            <th>Recebido em</th>
          </tr>
        </thead>
        <tbody>
          {eventos.map((evento) => {
            const situacao = derivarSituacao(evento);
            return (
              <tr key={evento.id} className={ACCENT_POR_SITUACAO[situacao]}>
                <td className="row-accent" aria-hidden="true"></td>
                <td className="mono">{evento.idTransacao}</td>
                <td className="mono">
                  <button
                    type="button"
                    className="link-button"
                    onClick={() => onSelecionarContrato(evento.idContrato)}
                    title="Ver status atual deste contrato"
                  >
                    {evento.idContrato}
                  </button>
                </td>
                <td className="mono">{formatarMoeda(evento.valor)}</td>
                <td>{formatarData(evento.dataPagamento)}</td>
                <td>
                  <div className="status-cell">
                    <StatusBadge status={evento.statusRecebido} />
                    {situacao === "pendente" && (
                      <span className="badge badge--pending" title="Aguardando processamento em background">
                        Processando…
                      </span>
                    )}
                    {situacao === "erro-processamento" && (
                      <span
                        className="alert-chip"
                        role="alert"
                        title={evento.erroProcessamento}
                      >
                        ⚠ Falha no processamento
                      </span>
                    )}
                  </div>
                </td>
                <td className="mono muted">{formatarData(evento.recebidoEm)}</td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}
