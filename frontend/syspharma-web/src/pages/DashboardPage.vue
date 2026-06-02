<script setup>
import { onMounted, ref } from 'vue';
import { getProducts } from '../api/products';
import PageHeader from '../components/PageHeader.vue';
import MetricCard from '../components/MetricCard.vue';
import InfoCard from '../components/InfoCard.vue';

const resumo = ref({
  totalProdutos: 0,
  produtosVencendo: 0,
  estoqueBaixo: 0,
  campanhasAtivas: 0
});

const carregando = ref(false);
const erro = ref('');
const alertasValidade = ref([]);

onMounted(async () => {
  carregando.value = true;
  erro.value = '';

  try {
    const produtos = await getProducts();

    resumo.value = {
      totalProdutos: produtos.length,
      produtosVencendo: 0,
      estoqueBaixo: produtos.filter((produto) => Number(produto.estoque) <= 10).length,
      campanhasAtivas: 0
    };

    alertasValidade.value = [];
  } catch {
    erro.value = 'Nao foi possivel carregar o resumo do dashboard.';
  } finally {
    carregando.value = false;
  }
});
</script>

<template>
  <section>
    <PageHeader
      title="Dashboard"
      subtitle="Resumo operacional da farmacia"
    />

    <p v-if="carregando" class="state-message">
      Carregando resumo...
    </p>

    <p v-else-if="erro" class="state-message error">
      {{ erro }}
    </p>

    <template v-else>
      <section class="metrics">
        <MetricCard
          label="Produtos cadastrados"
          :value="resumo.totalProdutos"
        />

        <MetricCard
          label="Proximos ao vencimento"
          :value="resumo.produtosVencendo"
          tone="warning"
        />

        <MetricCard
          label="Estoque baixo"
          :value="resumo.estoqueBaixo"
          tone="danger"
        />

        <MetricCard
          label="Campanhas ativas"
          :value="resumo.campanhasAtivas"
          tone="success"
        />
      </section>

      <section class="grid">
        <InfoCard title="Alertas de validade">
          <ul v-if="alertasValidade.length">
            <li v-for="alerta in alertasValidade" :key="alerta.id">
              {{ alerta.produto }} vence em {{ alerta.diasParaVencer }} dias
            </li>
          </ul>

          <p v-else class="empty-state">
            Nenhum produto proximo do vencimento.
          </p>
        </InfoCard>

        <InfoCard title="Proximas acoes">
          <ul>
            <li>Revisar estoque minimo</li>
            <li>Atualizar cadastro de produtos</li>
            <li>Conferir clientes recorrentes</li>
          </ul>
        </InfoCard>
      </section>
    </template>
  </section>
</template>

<style scoped>
.metrics {
  display: grid;
  grid-template-columns: repeat(4, minmax(160px, 1fr));
  gap: 16px;
  margin-bottom: 24px;
}

.grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}

ul {
  margin: 0;
  padding-left: 18px;
  color: #475569;
}

li + li {
  margin-top: 8px;
}

.empty-state {
  margin: 0;
  color: #64748b;
}

.state-message {
  margin: 0 0 18px;
  background: #fff;
  border: 1px solid #e1e7ef;
  border-radius: 8px;
  padding: 18px;
  color: #64748b;
}

.state-message.error {
  border-color: #fecaca;
  background: #fef2f2;
  color: #991b1b;
}

@media (max-width: 1000px) {
  .metrics,
  .grid {
    grid-template-columns: 1fr;
  }
}
</style>
