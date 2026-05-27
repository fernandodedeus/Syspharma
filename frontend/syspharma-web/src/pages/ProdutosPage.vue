<script setup>
import { onMounted, ref } from 'vue';
import { api } from '../api/http'; // Ajuste o caminho conforme necessário
import PageHeader from '../components/PageHeader.vue'; // Componente de cabeçalho reutilizável
import StatusBadge from '../components/StatusBadge.vue'; // Componente de badge de status reutilizável, para mostrar o estoque com cores diferentes

const produtos = ref([]);

const novoProduto = ref({
  nome: '',
  codigoBarras: '',
  precoVenda: 0,
  estoque: 0
});

async function carregarProdutos() {
  const response = await api.get('/produtos'); // Ajuste o endpoint conforme necessário
  produtos.value = response.data;
}

async function salvarProduto() {
  await api.post('/produtos', novoProduto.value); // Ajuste o endpoint conforme necessário

  novoProduto.value = {
    nome: '',
    codigoBarras: '',
    precoVenda: 0,
    estoque: 0
  };

  function obterQuantidadeEstoque(produto) {
  return produto.estoque?.quantidade ?? produto.estoque ?? 0;
    }

  function obterTomEstoque(produto) {
  return obterQuantidadeEstoque(produto) <= 10 ? 'low' : 'ok';
      }


  await carregarProdutos(); // Recarrega a lista de produtos após salvar um novo produto
}

onMounted(carregarProdutos);
</script>

<template>
  <section>
    <PageHeader
      title="Produtos"
      subtitle="Cadastro inicial para testar a interface"
    /> <!-- Uso do componente de cabeçalho, passando o título e a descrição como props -->


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
              <StatusBadge :tone="obterTomEstoque(produto)">
                        {{ obterQuantidadeEstoque(produto) }} un.
              </StatusBadge>
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