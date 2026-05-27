<script setup>
import { onMounted, ref } from 'vue';
import { api } from '../api/http';

const produtos = ref([]);

const novoProduto = ref({
  nome: '',
  codigoBarras: '',
  precoVenda: 0,
  estoque: 0
});

async function carregarProdutos() {
  const response = await api.get('/produtos');
  produtos.value = response.data;
}

async function salvarProduto() {
  await api.post('/produtos', novoProduto.value);

  novoProduto.value = {
    nome: '',
    codigoBarras: '',
    precoVenda: 0,
    estoque: 0
  };

  await carregarProdutos();
}

onMounted(carregarProdutos);
</script>

<template>
  <section>
    <header class="page-header">
      <div>
        <h1>Produtos</h1>
        <p>Cadastro inicial para testar a interface</p>
      </div>
    </header>

    <form class="card form" @submit.prevent="salvarProduto">
      <input v-model="novoProduto.nome" placeholder="Nome do produto" />
      <input v-model="novoProduto.codigoBarras" placeholder="Código de barras" />
      <input v-model.number="novoProduto.precoVenda" type="number" placeholder="Preço" />
      <input v-model.number="novoProduto.estoque" type="number" placeholder="Estoque" />
      <button type="submit">Salvar</button>
    </form>

    <section class="card">
      <table>
        <thead>
          <tr>
            <th>Produto</th>
            <th>Código</th>
            <th>Preço</th>
            <th>Estoque</th>
          </tr>
        </thead>

        <tbody>
          <tr v-for="produto in produtos" :key="produto.id">
            <td>{{ produto.nome }}</td>
            <td>{{ produto.codigoBarras }}</td>
            <td>R$ {{ produto.precoVenda.toFixed(2) }}</td>
            <td>
              <span :class="['badge', (produto.estoque?.quantidade ?? produto.estoque ?? 0) <= 10 ? 'low' : 'ok']">
                        {{ produto.estoque?.quantidade ?? produto.estoque ?? 0 }} un.
                </span>
            </td>
          </tr>
        </tbody>
      </table>
    </section>
  </section>
</template>

<style scoped>
.form {
  display: grid;
  grid-template-columns: 2fr 1.4fr 1fr 1fr auto;
  gap: 10px;
  margin-bottom: 18px;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th {
  color: #64748b;
  font-size: 13px;
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

.badge {
  display: inline-block;
  border-radius: 999px;
  padding: 5px 10px;
  font-size: 13px;
  font-weight: 700;
}

.badge.ok {
  background: #dcfce7;
  color: #166534;
}

.badge.low {
  background: #fee2e2;
  color: #991b1b;
}

@media (max-width: 1000px) {
  .form {
    grid-template-columns: 1fr;
  }

  table {
    display: block;
    overflow-x: auto;
  }
}
</style>