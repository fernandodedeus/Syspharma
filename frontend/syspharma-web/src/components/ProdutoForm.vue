<script setup>
import { computed, reactive } from 'vue';
import FormField from './FormField.vue'; // Componente de campo de formulário reutilizável

// emit significa ProdutoForm avisa a ProdutosPage: "O usuário enviou este produto"
const emit = defineEmits(['submit']); // envia um evento do componente filho para o componente pai 

const produto = reactive({ // cria um objeto reativo com os dados do formulário
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

function validarFormulario() {
  erros.nome = produto.nome.trim() ? '' : 'Informe o nome do produto.';
  erros.codigoBarras = produto.codigoBarras.trim() ? '' : 'Informe o código de barras.';
  erros.precoVenda = Number(produto.precoVenda) > 0 ? '' : 'Informe um preço maior que zero.';
  erros.estoque = Number(produto.estoque) >= 0 ? '' : 'O estoque não pode ser negativo.';

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
      label="Código de barras"
      :error="erros.codigoBarras"
    />

    <FormField
      id="precoVenda"
      v-model="produto.precoVenda"
      label="Preço (em R$)"
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

    <button class="form-button" type="submit" :disabled="!formularioValido">
              Salvar
    </button>

  </form>
</template>

<style scoped>
.form {
  display: grid;
  grid-template-columns: 2fr 1.4fr 1fr 1fr auto;
  align-items: end;
  gap: 10px;
  margin-bottom: 18px;
}

.form-button {
  min-height: 40px;
}

@media (max-width: 1000px) {
  .form {
    grid-template-columns: 1fr;
  }

  .form-button {
    width: 100%;
  }
}

.form-button:disabled {
  background: #94a3b8;
  cursor: not-allowed;
}
</style>