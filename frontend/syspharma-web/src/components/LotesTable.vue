<script setup>
import StatusBadge from './StatusBadge.vue';

defineProps({
  lotes: {
    type: Array,
    default: () => []
  }
});

// Define o tom do badge com base nos dias para vencer
function obterTomValidade(diasParaVencer) {
  if (diasParaVencer < 0) return 'low';       // já vencido
  if (diasParaVencer <= 30) return 'low';     // vermelho — menos de 30 dias
  if (diasParaVencer <= 90) return 'warning'; // amarelo — entre 30 e 90 dias
  return 'ok';                                // verde — acima de 90 dias
}

// Monta o texto do badge de forma legível
function obterTextoValidade(diasParaVencer) {
  if (diasParaVencer < 0) return `Vencido há ${Math.abs(diasParaVencer)} dias`;
  if (diasParaVencer === 0) return 'Vence hoje';
  return `Vence em ${diasParaVencer} dias`;
}

// Formata a data de validade para exibição no padrão brasileiro
function formatarData(dataIso) {
  if (!dataIso) return '—';

  /* A coluna Quantidade está com — por enquanto porque o 
  endpoint expiringbatches não retorna esse campo. 
  Quando precisar, vou trocar por lote.quantity */

  // A API retorna formato "YYYY-MM-DD", então montamos manualmente
  // para evitar problemas de fuso horário com o new Date()
  const [ano, mes, dia] = dataIso.split('-');
  return `${dia}/${mes}/${ano}`;
}
</script>

<template>
  <section class="card table-card">
    <table>
      <thead>
        <tr>
          <th>Produto</th>
          <th>Código do lote</th>
          <th>Validade</th>
          <th>Situação</th>
          <th>Quantidade</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="lote in lotes" :key="lote.idBatch">
          <td>
            <span class="product-name">{{ lote.productInternalCode }}</span>
            <small v-if="lote.description" class="product-description">
              {{ lote.description }}
            </small>
          </td>
          <td>{{ lote.batchCode }}</td>
          <td>{{ formatarData(lote.expirationDate ?? lote.expirationdate) }}</td>
          <td>
            <StatusBadge :tone="obterTomValidade(lote.expiresInDays)">
              {{ obterTextoValidade(lote.expiresInDays) }}
            </StatusBadge>
          </td>
          <td>—</td>
        </tr>
      </tbody>
    </table>
  </section>
</template>

<style scoped>
.table-card {
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th {
  color: #64748b;
  font-size: 13px;
  font-weight: 700;
  text-align: left;
}

td,
th {
  border-bottom: 1px solid #e5eaf0;
  padding: 14px 10px;
}

tbody tr:hover {
  background: #f8fafc;
}

.product-name {
  display: block;
  font-weight: 600;
  color: #172033;
}

.product-description {
  display: block;
  color: #64748b;
  font-size: 12px;
  margin-top: 2px;
}

@media (max-width: 1000px) {
  table {
    min-width: 600px;
  }
}
</style>