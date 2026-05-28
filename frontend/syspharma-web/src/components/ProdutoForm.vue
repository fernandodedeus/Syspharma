<script setup>
import { reactive } from 'vue';

// emit significa ProdutoForm avisa a ProdutosPage: "O usuário enviou este produto"
const emit = defineEmits(['submit']); // envia um evento do componente filho para o componente pai 

const produto = reactive({ // cria um objeto reativo com os dados do formulário
  nome: '',
  codigoBarras: '',
  precoVenda: 0,
  estoque: 0
});

function limparFormulario() {
  produto.nome = '';
  produto.codigoBarras = '';
  produto.precoVenda = 0;
  produto.estoque = 0;
}

function enviarFormulario() {
  emit('submit', {
    nome: produto.nome,
    codigoBarras: produto.codigoBarras,
    precoVenda: produto.precoVenda,
    estoque: produto.estoque
  });

  limparFormulario();
}
</script>

<template>
  <form class="card form" @submit.prevent="enviarFormulario">
    <label class="form-field" for="nome">
      <span>Nome do produto</span>
      <input id="nome" v-model="produto.nome" />
    </label>

    <label class="form-field" for="codigoBarras">
      <span>Código de barras</span>
      <input id="codigoBarras" v-model="produto.codigoBarras" />
    </label>

    <label class="form-field" for="precoVenda">
      <span>Preço (em R$)</span>
      <input id="precoVenda" v-model.number="produto.precoVenda" type="number" />
    </label>

    <label class="form-field" for="estoque">
      <span>Estoque</span>
      <input id="estoque" v-model.number="produto.estoque" type="number" />
    </label>

    <button class="form-button" type="submit">Salvar</button>
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

.form-field {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.form-field span {
  color: #475569;
  font-size: 13px;
  font-weight: 700;
}

.form-field input {
  width: 100%;
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
</style>