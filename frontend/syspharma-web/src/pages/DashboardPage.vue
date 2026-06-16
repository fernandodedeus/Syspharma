<script setup>
import { onMounted, ref } from 'vue';
import { getProducts } from '../api/products';
import { getExpiringBatches } from '../api/batches';
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
    // Busca produtos e lotes vencendo em paralelo
    const [produtos, lotes] = await Promise.all([
      getProducts(),
      getExpiringBatches()
    ]);

    // Filtra apenas lotes que vencem em até 30 dias
    const lotesUrgentes = lotes.filter(lote => lote.expiresInDays <= 30);

    resumo.value = {
      totalProdutos: produtos.length,
      produtosVencendo: lotesUrgentes.length,
      estoqueBaixo: produtos.filter((produto) => Number(produto.estoque) <= 10).length,
      campanhasAtivas: 0
    };

    // Monta a lista de alertas para o InfoCard
    alertasValidade.value = lotes
      .filter(lote => lote.expiresInDays <= 90) // mostra até 90 dias no painel
      .sort((a, b) => a.expiresInDays - b.expiresInDays) // ordena do mais urgente
      .slice(0, 5) // limita a 5 alertas no dashboard
      .map(lote => ({
        id: lote.idBatch,
        produto: lote.description ?? lote.productInternalCode,
        codigoLote: lote.batchCode,
        diasParaVencer: lote.expiresInDays
      }));

  } catch {
    erro.value = 'Não foi possível carregar o resumo do dashboard.';
  } finally {
    carregando.value = false;
  }
});

// Retorna a classe CSS do alerta com base nos dias para vencer
function classeAlerta(dias) {
  if (dias < 0) return 'alerta-vencido';
  if (dias <= 30) return 'alerta-urgente';
  return 'alerta-atencao';
}

// Texto legível dos dias para vencer
function textoValidade(dias) {
  if (dias < 0) return `Vencido há ${Math.abs(dias)} dias`;
  if (dias === 0) return 'Vence hoje';
  return `Vence em ${dias} dias`;
}
</script>

<template>
  <section>
    <PageHeader
      title="Dashboard"
      subtitle="Resumo operacional da farmácia"
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
          <ul v-if="alertasValidade.length" class="alerta-lista">
            <li
              v-for="alerta in alertasValidade"
              :key="alerta.id"
              :class="['alerta-item', classeAlerta(alerta.diasParaVencer)]"
            >
              <span class="alerta-produto">{{ alerta.produto }}</span>
              <span class="alerta-lote">Lote: {{ alerta.codigoLote }}</span>
              <span class="alerta-dias">{{ textoValidade(alerta.diasParaVencer) }}</span>
            </li>
          </ul>

          <p v-else class="empty-state">
            Nenhum produto próximo do vencimento.
          </p>
        </InfoCard>

        <InfoCard title="Próximas ações">
          <ul>
            <li>Revisar estoque mínimo</li>
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

.alerta-lista {
  margin: 0;
  padding: 0;
  list-style: none;
  display: grid;
  gap: 10px;
}

.alerta-item {
  display: grid;
  grid-template-columns: 1fr auto;
  grid-template-rows: auto auto;
  gap: 2px 12px;
  padding: 10px 12px;
  border-radius: 8px;
  border-left: 4px solid transparent;
}

.alerta-produto {
  font-weight: 600;
  font-size: 14px;
  color: #172033;
}

.alerta-lote {
  font-size: 12px;
  color: #64748b;
  grid-column: 1;
}

.alerta-dias {
  grid-row: 1 / 3;
  grid-column: 2;
  align-self: center;
  font-size: 12px;
  font-weight: 700;
  white-space: nowrap;
}

/* Tons dos alertas */
.alerta-vencido {
  background: #fef2f2;
  border-color: #dc2626;
  color: #991b1b;
}

.alerta-vencido .alerta-dias {
  color: #dc2626;
}

.alerta-urgente {
  background: #fef2f2;
  border-color: #f97316;
  color: #172033;
}

.alerta-urgente .alerta-dias {
  color: #ea580c;
}

.alerta-atencao {
  background: #fefce8;
  border-color: #eab308;
  color: #172033;
}

.alerta-atencao .alerta-dias {
  color: #ca8a04;
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