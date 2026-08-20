# Sabemi · Painel de Pagamentos (Frontend)

Dashboard administrativo em React + Vite para acompanhar as notificações de
pagamento recebidas via webhook.

## Rodando localmente

```bash
npm install
npm run dev
```

A aplicação sobe em `http://localhost:5173` por padrão (já liberado no CORS
da API).

## Configuração

A URL da API é lida da variável de ambiente `VITE_API_URL` (arquivo `.env`).
Por padrão aponta para `http://localhost:5065` — ajuste conforme a porta da
sua API .NET.

## Funcionalidades

- Listagem dos pagamentos recebidos, com atualização automática a cada 5s
  (polling) e botão de atualização manual.
- Filtros por status (Sucesso/Erro) e por ID do contrato (com debounce).
- Alerta visual claro quando um evento falha no processamento interno
  (borda vermelha na linha + selo "Falha no processamento" com a mensagem
  de erro em tooltip), independente do status reportado pelo banco.
- Indicador de "Processando…" enquanto o evento aguarda o worker em
  background (janela de ~2s simulando a regra de negócio pesada).
- Consulta rápida do status atual de um contrato (clique no ID do contrato
  na tabela ou digite manualmente no painel lateral).

## Build de produção

```bash
npm run build
```

Gera os arquivos estáticos em `dist/`.
