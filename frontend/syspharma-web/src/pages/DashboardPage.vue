<script setup>
import { onMounted, ref } from 'vue';
import { api } from '../api/http'; // Ajuste o caminho conforme necessário
import PageHeader from '../components/PageHeader.vue'; // Componente de cabeçalho reutilizável
import MetricCard from '../components/MetricCard.vue'; // Componente de card de métrica reutilizável, para mostrar os números principais do dashboard
import InfoCard from '../components/InfoCard.vue'; // Componente de card de informação reutilizável, para mostrar listas ou detalhes no dashboard

const resumo = ref({
  totalProdutos: 0,
  produtosVencendo: 0,
  estoqueBaixo: 0,
  campanhasAtivas: 0
});

const alertasValidade = ref([ // Esses dados estão fixos aqui só para mostrar como seria a interface, na prática eles deveriam vir da API, assim como os números do resumo lá em cima. /
  {
    id: 1,
    produto: 'Dipirona 500mg',
    diasParaVencer: 0
  },
  {
    id: 2,
    produto: 'Soro fisiológico',
    diasParaVencer: 0
  },
  {
    id: 3,
    produto: 'Vitamina C',
    diasParaVencer: 0
  }
]);

onMounted(async () => {
  const response = await api.get('/dashboard/resumo'); // Ajuste o endpoint conforme necessário
  resumo.value = {
    ...resumo.value,
    ...response.data
  };
});
</script>

<template>
  <section>

      <PageHeader 
        title="Dashboard"
        subtitle="Resumo operacional da farmácia"
      /> <!-- Uso do componente de cabeçalho, passando o título e a descrição como props -->




    <section class="metrics"> <!-- área de métricas principais, usando o componente MetricCard para mostrar os números mais importantes do dashboard -->

      <MetricCard
        label="Produtos cadastrados"
        :value="resumo.totalProdutos" 
      /> <!-- O : significa que estamos passando uma variável JavaScript, não um texto fixo -->

      <MetricCard
        label="Próximos ao vencimento"
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
            Nenhum produto próximo do vencimento.
          </p>

      </InfoCard>

      <InfoCard title="Próximas ações">
        <ul>
          <li>Revisar estoque mínimo</li>
          <li>Criar campanha para produtos próximos do vencimento</li>
          <li>Atualizar cadastro de clientes recorrentes</li>
        </ul>
      </InfoCard>
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
  min-height: 126px;
  background: #fff;
  border: 1px solid #e1e7ef;
  border-left: 5px solid #1f8a70;
  border-radius: 8px;
  padding: 18px;

  display: flex;
  flex-direction: column;
  justify-content: space-between;
  gap: 18px;
}

.metric-card span {
  display: block;
  color: #475569;
  font-size: 14px;
  font-weight: 700;
  line-height: 1.35;
}

.metric-card strong {
  font-size: clamp(26px, 3vw, 34px);
  line-height: 1.1;
  overflow-wrap: anywhere;
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

.empty-state {
  margin: 0;
  color: #64748b;
}

</style>