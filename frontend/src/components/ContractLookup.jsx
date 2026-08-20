import { useEffect, useState } from "react";
import { consultarStatusContrato } from "../api/client";
import StatusBadge from "./StatusBadge";
import { formatarMoeda, formatarData } from "../utils/format";

export default function ContractLookup({ idContratoSelecionado }) {
  const [idContrato, setIdContrato] = useState("");
  const [resultado, setResultado] = useState(null);
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState(null);

  async function buscar(id) {
    const alvo = (id ?? idContrato).trim();
    if (!alvo) return;

    setCarregando(true);
    setErro(null);
    setResultado(null);

    try {
      const dados = await consultarStatusContrato(alvo);
      setResultado(dados);
    } catch (err) {
      if (err.response?.status === 404) {
        setErro("Nenhum status encontrado para este contrato ainda.");
      } else {
        setErro("Não foi possível consultar o contrato agora.");
      }
    } finally {
      setCarregando(false);
    }
  }

  useEffect(() => {
    if (idContratoSelecionado) {
      setIdContrato(idContratoSelecionado);
      buscar(idContratoSelecionado);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [idContratoSelecionado]);

  return (
    <aside className="lookup-card">
      <h2>Consultar contrato</h2>
      <p className="lookup-card__hint">
        Veja o último status de liquidação registrado para um contrato específico.
      </p>

      <form
        className="lookup-card__form"
        onSubmit={(e) => {
          e.preventDefault();
          buscar();
        }}
      >
        <input
          type="text"
          placeholder="ex: CONTR-123"
          value={idContrato}
          onChange={(e) => setIdContrato(e.target.value)}
        />
        <button type="submit" className="btn btn--primary" disabled={carregando}>
          {carregando ? "Buscando…" : "Buscar"}
        </button>
      </form>

      {erro && <p className="lookup-card__erro">{erro}</p>}

      {resultado && (
        <dl className="lookup-card__result">
          <div>
            <dt>Contrato</dt>
            <dd className="mono">{resultado.idContrato}</dd>
          </div>
          <div>
            <dt>Status atual</dt>
            <dd><StatusBadge status={resultado.statusAtual} /></dd>
          </div>
          <div>
            <dt>Última transação</dt>
            <dd className="mono">{resultado.ultimoIdTransacao}</dd>
          </div>
          <div>
            <dt>Valor pago</dt>
            <dd className="mono">{formatarMoeda(resultado.valorPago)}</dd>
          </div>
          <div>
            <dt>Data do pagamento</dt>
            <dd>{formatarData(resultado.dataUltimoPagamento)}</dd>
          </div>
          <div>
            <dt>Atualizado em</dt>
            <dd className="muted">{formatarData(resultado.atualizadoEm)}</dd>
          </div>
        </dl>
      )}
    </aside>
  );
}
