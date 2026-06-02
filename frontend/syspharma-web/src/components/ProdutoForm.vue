<script setup>
import { computed, reactive, watch } from 'vue';
import FormField from './FormField.vue';

const props = defineProps({
  initialProduct: {
    type: Object,
    default: null
  },
  loading: {
    type: Boolean,
    default: false
  }
});

const emit = defineEmits(['submit', 'cancel']);

const produto = reactive({
  nome: '',
  codigoBarras: '',
  precoVenda: 0,
  estoque: 0
});

const erros = reactive({
  nome: '',
  codigoBarras: '',
  precoVenda: '',
  estoque: ''
});

const formularioValido = computed(() => {
  return (
    produto.nome.trim() &&
    produto.codigoBarras.trim() &&
    Number(produto.precoVenda) > 0 &&
    Number(produto.estoque) >= 0
  );
});

watch(
  () => props.initialProduct,
  (product) => {
    if (!product) {
      limparFormulario();
      return;
    }

    produto.nome = product.nome;
    produto.codigoBarras = product.codigoBarras;
    produto.precoVenda = product.precoVenda;
    produto.estoque = product.estoque;
  },
  { immediate: true }
);

function validarFormulario() {
  erros.nome = produto.nome.trim() ? '' : 'Informe o nome do produto.';
  erros.codigoBarras = produto.codigoBarras.trim() ? '' : 'Informe o codigo.';
  erros.precoVenda = Number(produto.precoVenda) > 0 ? '' : 'Informe um preco maior que zero.';
  erros.estoque = Number(produto.estoque) >= 0 ? '' : 'O estoque nao pode ser negativo.';

  return formularioValido.value;
}

function limparFormulario() {
  produto.nome = '';
  produto.codigoBarras = '';
  produto.precoVenda = 0;
  produto.estoque = 0;

  erros.nome = '';
  erros.codigoBarras = '';
  erros.precoVenda = '';
  erros.estoque = '';
}

function enviarFormulario() {
  if (!validarFormulario()) {
    return;
  }

  emit('submit', {
    nome: produto.nome.trim(),
    codigoBarras: produto.codigoBarras.trim(),
    precoVenda: Number(produto.precoVenda),
    estoque: Number(produto.estoque)
  });

  if (!props.initialProduct) {
    limparFormulario();
  }
}

function cancelarEdicao() {
  emit('cancel');
  limparFormulario();
}
</script>

<template>
  <form class="card form" @submit.prevent="enviarFormulario">
    <FormField
      id="nome"
      v-model="produto.nome"
      label="Nome do produto"
      :error="erros.nome"
    />

    <FormField
      id="codigoBarras"
      v-model="produto.codigoBarras"
      label="Codigo"
      :error="erros.codigoBarras"
    />

    <FormField
      id="precoVenda"
      v-model="produto.precoVenda"
      label="Preco (R$)"
      type="number"
      :error="erros.precoVenda"
    />

    <FormField
      id="estoque"
      v-model="produto.estoque"
      label="Estoque"
      type="number"
      :error="erros.estoque"
    />

    <button class="form-button" type="submit" :disabled="!formularioValido || loading">
      {{ loading ? 'Salvando...' : initialProduct ? 'Atualizar' : 'Salvar' }}
    </button>

    <button
      v-if="initialProduct"
      class="form-button secondary"
      type="button"
      @click="cancelarEdicao"
    >
      Cancelar
    </button>
  </form>
</template>

<style scoped>
.form {
  display: grid;
  grid-template-columns: 2fr 1.4fr 1fr 1fr auto auto;
  align-items: end;
  gap: 10px;
  margin-bottom: 18px;
}

.form-button {
  min-height: 40px;
}

.form-button.secondary {
  background: #e2e8f0;
  color: #172033;
}

.form-button.secondary:hover {
  background: #cbd5e1;
}

.form-button:disabled {
  background: #94a3b8;
  cursor: not-allowed;
}

@media (max-width: 1000px) {
  .form {
    grid-template-columns: 1fr;
  }

  .form-button {
    width: 100%;
  }
}
</style>
