export default function Header({ segundosDesdeAtualizacao, aoVivo, onRefresh, atualizando }) {
  return (
    <header className="app-header">
      <div className="app-header__title">
        <span className="app-header__eyebrow">Sabemi</span>
        <h1>Painel de Pagamentos</h1>
      </div>

      <div className="app-header__status">
        <span className={`pulse ${aoVivo ? "pulse--live" : ""}`} aria-hidden="true" />
        <span className="app-header__status-text">
          {atualizando
            ? "Atualizando…"
            : segundosDesdeAtualizacao === null
            ? "Carregando dados"
            : `Atualizado há ${segundosDesdeAtualizacao}s`}
        </span>
        <button
          type="button"
          className="btn btn--ghost"
          onClick={onRefresh}
          disabled={atualizando}
        >
          Atualizar agora
        </button>
      </div>
    </header>
  );
}
