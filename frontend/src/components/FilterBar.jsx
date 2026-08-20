export default function FilterBar({ status, idContrato, onStatusChange, onContratoChange, onLimpar }) {
  const temFiltro = status || idContrato;

  return (
    <div className="filter-bar">
      <div className="filter-bar__field">
        <label htmlFor="filtro-status">Status do pagamento</label>
        <select
          id="filtro-status"
          value={status}
          onChange={(e) => onStatusChange(e.target.value)}
        >
          <option value="">Todos</option>
          <option value="Sucesso">Sucesso</option>
          <option value="Erro">Erro</option>
        </select>
      </div>

      <div className="filter-bar__field">
        <label htmlFor="filtro-contrato">ID do contrato</label>
        <input
          id="filtro-contrato"
          type="text"
          placeholder="ex: CONTR-123"
          value={idContrato}
          onChange={(e) => onContratoChange(e.target.value)}
        />
      </div>

      {temFiltro && (
        <button type="button" className="btn btn--text" onClick={onLimpar}>
          Limpar filtros
        </button>
      )}
    </div>
  );
}
