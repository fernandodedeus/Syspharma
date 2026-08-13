<script setup>
import { computed } from 'vue';

const props = defineProps({
  itens: {
    type: Array,
    default: () => []
  },
  desconto: {
    type: Number,
    default: 0
  },
  formaPagamento: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:desconto', 'update:formaPagamento']);

const formasPagamento = [
  { valor: 'dinheiro', label: 'Dinheiro' },
  { valor: 'cartao',   label: 'Cartão' },
  { valor: 'pix',      label: 'PIX' },
  { valor: 'boleto',   label: 'Boleto' }
];

const subtotal = computed(() => {
  return props.itens.reduce((acc, item) => acc + item.total, 0);
});

const total = computed(() => {
  const resultado = subtotal.value - Number(props.desconto ?? 0);
  return resultado < 0 ? 0 : resultado;
});

function formatarPreco(valor) {
  return Number(valor ?? 0).toLocaleString('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  });
}
</script>

<template>
  <div class="resumo">
    <h3 class="section-title">Resumo</h3>

    <!-- Totais -->
    <div class="totais">
      <div class="totais-row">
        <span>Subtotal</span>
        <span>{{ formatarPreco(subtotal) }}</span>
      </div>

      <div class="totais-row">
        <span>Desconto (R$)</span>
        <input
          class="desconto-input"
          type="number"
          min="0"
          step="0.01"
          placeholder="0,00"
          :value="desconto"
          @input="emit('update:desconto', Number($event.target.value))"
        />
      </div>

      <div class="totais-row total">
        <span>Total</span>
        <span>{{ formatarPreco(total) }}</span>
      </div>
    </div>

    <div class="divider" />

    <!-- Forma de pagamento -->
    <div class="pagamento">
      <span class="field-label">Forma de pagamento</span>

      <div class="pagamento-opcoes">
        <button
          v-for="forma in formasPagamento"
          :key="forma.valor"
          :class="['pagamento-btn', { ativo: formaPagamento === forma.valor }]"
          type="button"
          @click="emit('update:formaPagamento', forma.valor)"
        >
          {{ forma.label }}
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.section-title {
  font-size: 14px;
  font-weight: 700;
  color: #172033;
  margin: 0 0 12px;
}

/* Totais */
.totais {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.totais-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 14px;
  color: #475569;
}

.totais-row.total {
  font-size: 18px;
  font-weight: 700;
  color: #172033;
  margin-top: 4px;
}

.desconto-input {
  width: 100px;
  border: 1px solid #cfd8e3;
  border-radius: 8px;
  padding: 6px 10px;
  font-size: 14px;
  text-align: right;
  outline: none;
}

.desconto-input:focus {
  border-color: #1f8a70;
}

.divider {
  height: 1px;
  background: #e1e7ef;
  margin: 16px 0;
}

/* Forma de pagamento */
.field-label {
  display: block;
  font-size: 12px;
  font-weight: 700;
  color: #475569;
  margin-bottom: 10px;
}

.pagamento-opcoes {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 8px;
}

.pagamento-btn {
  padding: 10px;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 600;
  background: #f1f5f9;
  color: #475569;
  border: 2px solid transparent;
  cursor: pointer;
  transition: all 0.15s;
}

.pagamento-btn:hover {
  background: #e2e8f0;
}

.pagamento-btn.ativo {
  background: #f0fdf4;
  border-color: #1f8a70;
  color: #1f8a70;
}
</style>