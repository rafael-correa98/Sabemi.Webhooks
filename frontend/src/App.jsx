import { useCallback, useEffect, useRef, useState } from "react";
import Header from "./components/Header";
import FilterBar from "./components/FilterBar";
import PaymentsTable from "./components/PaymentsTable";
import ContractLookup from "./components/ContractLookup";
import { listarPagamentos } from "./api/client";
import "./index.css";

const INTERVALO_POLLING_MS = 5000;

export default function App() {
  const [eventos, setEventos] = useState([]);
  const [status, setStatus] = useState("");
  const [idContratoFiltro, setIdContratoFiltro] = useState("");
  const [idContratoSelecionado, setIdContratoSelecionado] = useState("");
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState(null);
  const [segundosDesdeAtualizacao, setSegundosDesdeAtualizacao] = useState(null);

  const ultimaAtualizacaoRef = useRef(null);
  const debounceRef = useRef(null);

  const buscarPagamentos = useCallback(async (filtros) => {
    setCarregando(true);
    setErro(null);
    try {
      const resultado = await listarPagamentos(filtros);
      setEventos(resultado.itens);
      ultimaAtualizacaoRef.current = Date.now();
      setSegundosDesdeAtualizacao(0);
    } catch (err) {
      setErro(
        err.code === "ERR_NETWORK"
          ? "Não foi possível conectar à API. Verifique se o backend está rodando."
          : "Ocorreu um erro ao buscar os pagamentos."
      );
    } finally {
      setCarregando(false);
    }
  }, []);

  // Busca inicial + refetch quando o filtro de status muda (imediato)
  useEffect(() => {
    buscarPagamentos({ status, idContrato: idContratoFiltro });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [status]);

  // Debounce no filtro de texto (contrato) para não disparar 1 request por tecla
  useEffect(() => {
    clearTimeout(debounceRef.current);
    debounceRef.current = setTimeout(() => {
      buscarPagamentos({ status, idContrato: idContratoFiltro });
    }, 350);
    return () => clearTimeout(debounceRef.current);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [idContratoFiltro]);

  // Polling automático
  useEffect(() => {
    const intervalo = setInterval(() => {
      buscarPagamentos({ status, idContrato: idContratoFiltro });
    }, INTERVALO_POLLING_MS);
    return () => clearInterval(intervalo);
  }, [status, idContratoFiltro, buscarPagamentos]);

  // Contador de "atualizado há Xs"
  useEffect(() => {
    const tick = setInterval(() => {
      if (ultimaAtualizacaoRef.current) {
        setSegundosDesdeAtualizacao(Math.floor((Date.now() - ultimaAtualizacaoRef.current) / 1000));
      }
    }, 1000);
    return () => clearInterval(tick);
  }, []);

  return (
    <div className="app-shell">
      <Header
        segundosDesdeAtualizacao={segundosDesdeAtualizacao}
        aoVivo={!erro}
        atualizando={carregando}
        onRefresh={() => buscarPagamentos({ status, idContrato: idContratoFiltro })}
      />

      <main className="app-main">
        <section className="app-main__primary">
          <FilterBar
            status={status}
            idContrato={idContratoFiltro}
            onStatusChange={setStatus}
            onContratoChange={setIdContratoFiltro}
            onLimpar={() => {
              setStatus("");
              setIdContratoFiltro("");
            }}
          />

          <PaymentsTable
            eventos={eventos}
            carregando={carregando}
            erro={erro}
            onSelecionarContrato={setIdContratoSelecionado}
          />
        </section>

        <ContractLookup idContratoSelecionado={idContratoSelecionado} />
      </main>
    </div>
  );
}
