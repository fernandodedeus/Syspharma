<script setup>
import { onMounted, ref } from 'vue';
import { api } from '../api/http';

const resumo = ref({
  totalProdutos: 0,
  produtosVencendo: 0,
  estoqueBaixo: 0,
  campanhasAtivas: 0
});

onMounted(async () => {
  const response = await api.get('/dashboard/resumo');
  resumo.value = {
    ...resumo.value,
    ...response.data
  };
});
</script>

<template>
  <section>
    <header class="page-header">
      <div>
        <h1>Dashboard</h1>
        <p>Resumo operacional da farmácia</p>
      </div>
    </header>

    <section class="metrics">
      <article class="metric-card">
        <span>Produtos cadastrados</span>
        <strong>{{ resumo.totalProdutos }}</strong>
      </article>

      <article class="metric-card warning">
        <span>Próximos ao vencimento</span>
        <strong>{{ resumo.produtosVencendo }}</strong>
      </article>

      <article class="metric-card danger">
        <span>Estoque baixo</span>
        <strong>{{ resumo.estoqueBaixo }}</strong>
      </article>

      <article class="metric-card success">
        <span>Campanhas ativas</span>
        <strong>{{ resumo.campanhasAtivas }}</strong>
      </article>
    </section>

    <section class="grid">
      <article class="card">
        <h2>Alertas de validade</h2>
        <ul>
          <li>Dipirona 500mg vence em 8 dias</li>
          <li>Soro fisiológico vence em 15 dias</li>
          <li>Vitamina C vence em 22 dias</li>
        </ul>
      </article>

      <article class="card">
        <h2>Próximas ações</h2>
        <ul>
          <li>Revisar estoque mínimo</li>
          <li>Criar campanha para produtos próximos do vencimento</li>
          <li>Atualizar cadastro de clientes recorrentes</li>
        </ul>
      </article>
    </section>
  </section>
</template>

<style scoped>
.metrics {
  display: grid;
  grid-template-columns: repeat(4, minmax(160px, 1fr));
  gap: 16px;
  margin-bottom: 24px;
}

.metric-card {
  background: #fff;
  border: 1px solid #e1e7ef;
  border-left: 5px solid #1f8a70;
  border-radius: 8px;
  padding: 18px;
}

.metric-card span {
  display: block;
  color: #64748b;
  font-size: 14px;
  margin-bottom: 12px;
}

.metric-card strong {
  font-size: 34px;
}

.metric-card.warning {
  border-left-color: #d97706;
}

.metric-card.danger {
  border-left-color: #dc2626;
}

.metric-card.success {
  border-left-color: #16a34a;
}

.grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}

h2 {
  margin: 0 0 14px;
  font-size: 18px;
}

ul {
  margin: 0;
  padding-left: 18px;
  color: #475569;
}

li + li {
  margin-top: 8px;
}

@media (max-width: 1000px) {
  .metrics,
  .grid {
    grid-template-columns: 1fr;
  }
}
</style>