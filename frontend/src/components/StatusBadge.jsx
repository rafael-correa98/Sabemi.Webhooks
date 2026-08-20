const VARIANTS = {
  sucesso: { label: "Sucesso", className: "badge badge--success" },
  erro: { label: "Erro", className: "badge badge--error" },
  pendente: { label: "Pendente", className: "badge badge--pending" },
  neutro: { label: null, className: "badge badge--neutral" },
};

/**
 * Badge de status reportado pelo banco (statusRecebido), independente
 * de o processamento interno ter falhado ou não.
 */
export default function StatusBadge({ status }) {
  const chave = (status || "").trim().toLowerCase();
  const variante =
    chave === "sucesso"
      ? VARIANTS.sucesso
      : chave === "erro"
      ? VARIANTS.erro
      : { label: status || "—", className: "badge badge--neutral" };

  return <span className={variante.className}>{variante.label}</span>;
}
