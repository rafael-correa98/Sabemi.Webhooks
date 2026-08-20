export function formatarMoeda(valor) {
  return new Intl.NumberFormat("pt-BR", {
    style: "currency",
    currency: "BRL",
  }).format(valor ?? 0);
}

export function formatarData(isoString) {
  if (!isoString) return "—";
  const data = new Date(isoString);
  return new Intl.DateTimeFormat("pt-BR", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  }).format(data);
}

/**
 * Deriva a "situação" visual da linha a partir dos dados brutos do evento.
 * Prioridade: falha de processamento > pendente > sucesso confirmado > neutro.
 */
export function derivarSituacao(evento) {
  if (evento.erroProcessamento) return "erro-processamento";
  if (!evento.processado) return "pendente";
  if ((evento.statusRecebido || "").toLowerCase() === "sucesso") return "sucesso";
  return "neutro";
}
