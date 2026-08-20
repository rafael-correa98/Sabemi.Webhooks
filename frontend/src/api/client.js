import axios from "axios";

const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL || "http://localhost:5065",
  timeout: 10000,
});

/**
 * Lista os pagamentos recebidos, com filtros opcionais de status e contrato.
 */
export async function listarPagamentos({ status, idContrato, pagina = 1, tamanhoPagina = 20 } = {}) {
  const params = { pagina, tamanhoPagina };
  if (status) params.status = status;
  if (idContrato) params.idContrato = idContrato;

  const { data } = await apiClient.get("/pagamentos", { params });
  return data;
}

/**
 * Consulta o status atual de um contrato específico.
 */
export async function consultarStatusContrato(idContrato) {
  const { data } = await apiClient.get(`/contratos/${encodeURIComponent(idContrato)}/status`);
  return data;
}

export default apiClient;
