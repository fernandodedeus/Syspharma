<script setup>
import { ref, computed, watch } from 'vue';
import NfceItensForm from './NfceItensForm.vue';
import NfceResumo from './NfceResumo.vue';

const props = defineProps({
  aberto: {
    type: Boolean,
    default: false
  }
});

const emit = defineEmits(['fechar']);

// Estado da NFC-e
const itens = ref([]);
const desconto = ref(0);
const formaPagamento = ref('');
const cpfConsumidor = ref('');

// Computed para habilitar o botão de emitir
const podeEmitir = computed(() => {
  return (
    itens.value.length > 0 &&
    formaPagamento.value !== ''
  );
});

const total = computed(() => {
  const subtotal = itens.value.reduce((acc, item) => acc + item.total, 0);
  const resultado = subtotal - Number(desconto.value ?? 0);
  return resultado < 0 ? 0 : resultado;
});

function formatarPreco(valor) {
  return Number(valor ?? 0).toLocaleString('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  });
}

// Limpa o estado ao fechar o painel
watch(() => props.aberto, (novoValor) => {
  if (!novoValor) {
    setTimeout(() => {
      itens.value = [];
      desconto.value = 0;
      formaPagamento.value = '';
      cpfConsumidor.value = '';
    }, 300); // aguarda a animação de fechar
  }
});

function fechar() {
  emit('fechar');
}

function emitirNfce() {
  // Por enquanto apenas exibe um alerta
  // A integração com o backend será feita futuramente
  alert('Emissão de NFC-e será integrada com o backend em breve.');
}
</script>

<template>
  <Teleport to="body">
    <!-- Overlay escuro -->
    <Transition name="overlay">
      <div
        v-if="aberto"
        class="overlay"
        @click="fechar"
      />
    </Transition>

    <!-- Painel lateral -->
    <Transition name="panel">
      <div v-if="aberto" class="panel" role="dialog" aria-modal="true">

        <!-- Cabeçalho do painel -->
        <div class="panel-header">
          <div>
            <h2 class="panel-title">Nova NFC-e</h2>
            <p class="panel-subtitle">Nota Fiscal de Consumidor Eletrônica</p>
          </div>

          <button class="btn-fechar" type="button" @click="fechar">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="20"
              height="20"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
            >
              <line x1="18" y1="6" x2="6" y2="18"/>
              <line x1="6" y1="6" x2="18" y2="18"/>
            </svg>
          </button>
        </div>

        <!-- Corpo com scroll -->
        <div class="panel-body">

          <!-- CPF do consumidor (opcional) -->
          <div class="section">
            <label class="field-label" for="cpfConsumidor">
              CPF do consumidor
              <span class="field-opcional">(opcional)</span>
            </label>
            <input
              id="cpfConsumidor"
              v-model="cpfConsumidor"
              type="text"
              placeholder="000.000.000-00"
              maxlength="14"
            />
          </div>

          <div class="divider" />

          <!-- Itens -->
          <div class="section">
            <NfceItensForm
              :itens="itens"
              @update:itens="itens = $event"
            />
          </div>

          <div class="divider" />

          <!-- Resumo e pagamento -->
          <div class="section">
            <NfceResumo
              :itens="itens"
              :desconto="desconto"
              :forma-pagamento="formaPagamento"
              @update:desconto="desconto = $event"
              @update:forma-pagamento="formaPagamento = $event"
            />
          </div>

        </div>

        <!-- Rodapé fixo com total e botão de emitir -->
        <div class="panel-footer">
          <div class="footer-total">
            <span>Total</span>
            <strong>{{ formatarPreco(total) }}</strong>
          </div>

          <button
            class="btn-emitir"
            type="button"
            :disabled="!podeEmitir"
            @click="emitirNfce"
          >
            Emitir NFC-e
          </button>
        </div>

      </div>
    </Transition>
  </Teleport>
</template>

<style scoped>
/* Overlay */
.overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.5);
  z-index: 300;
}

/* Painel */
.panel {
  position: fixed;
  top: 0;
  right: 0;
  bottom: 0;
  width: min(480px, 100vw);
  background: #fff;
  z-index: 301;
  display: flex;
  flex-direction: column;
  box-shadow: -8px 0 32px rgba(0, 0, 0, 0.12);
}

/* Cabeçalho */
.panel-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  padding: 24px 24px 16px;
  border-bottom: 1px solid #e1e7ef;
  flex-shrink: 0;
}

.panel-title {
  font-size: 18px;
  font-weight: 700;
  color: #172033;
  margin: 0 0 4px;
}

.panel-subtitle {
  font-size: 12px;
  color: #64748b;
  margin: 0;
}

.btn-fechar {
  background: #f1f5f9;
  color: #475569;
  border-radius: 8px;
  padding: 6px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.btn-fechar:hover {
  background: #e2e8f0;
}

/* Corpo com scroll */
.panel-body {
  flex: 1;
  overflow-y: auto;
  padding: 0;
}

.section {
  padding: 20px 24px;
}

.divider {
  height: 1px;
  background: #e1e7ef;
}

/* Campos */
.field-label {
  display: block;
  font-size: 12px;
  font-weight: 700;
  color: #475569;
  margin-bottom: 6px;
}

.field-opcional {
  font-weight: 400;
  color: #94a3b8;
  margin-left: 4px;
}

input {
  width: 100%;
  border: 1px solid #cfd8e3;
  border-radius: 8px;
  padding: 10px 12px;
  font-size: 14px;
  outline: none;
}

input:focus {
  border-color: #1f8a70;
}

/* Rodapé */
.panel-footer {
  border-top: 1px solid #e1e7ef;
  padding: 16px 24px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 16px;
  flex-shrink: 0;
  background: #fff;
}

.footer-total {
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.footer-total span {
  font-size: 12px;
  color: #64748b;
}

.footer-total strong {
  font-size: 20px;
  font-weight: 700;
  color: #172033;
}

.btn-emitir {
  padding: 12px 24px;
  font-size: 14px;
  white-space: nowrap;
}

.btn-emitir:disabled {
  background: #94a3b8;
  cursor: not-allowed;
}

/* Animações */
.overlay-enter-active,
.overlay-leave-active {
  transition: opacity 0.25s ease;
}

.overlay-enter-from,
.overlay-leave-to {
  opacity: 0;
}

.panel-enter-active,
.panel-leave-active {
  transition: transform 0.3s ease;
}

.panel-enter-from,
.panel-leave-to {
  transform: translateX(100%);
}

/* Mobile — painel ocupa tela toda */
@media (max-width: 600px) {
  .panel {
    width: 100vw;
  }
}
</style>