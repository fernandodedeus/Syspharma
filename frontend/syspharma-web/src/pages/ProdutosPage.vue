<script setup>
import { computed, onMounted, ref } from 'vue';
import {
  createProduct,
  deleteProduct,
  getProducts,
  updateProduct
} from '../api/products';
import PageHeader from '../components/PageHeader.vue';
import ProdutoForm from '../components/ProdutoForm.vue';
import ProdutosTable from '../components/ProdutosTable.vue';
import ConfirmModal from '../components/ConfirmModal.vue';

const produtos = ref([]);
const busca = ref('');
const carregando = ref(false);
const salvando = ref(false);
const erro = ref('');
const produtoEmEdicao = ref(null);

// Exclusão de produtos
const mostrarModalExcluir = ref(false);
const produtoSelecionado = ref(null);

const produtosFiltrados = computed(() => {
  const termo = busca.value.trim().toLowerCase();

  if (!termo) {
    return produtos.value;
  }

  return produtos.value.filter((produto) => {
    return (
      produto.nome.toLowerCase().includes(termo) ||
      produto.codigoBarras.toLowerCase().includes(termo)
    );
  });
});

async function carregarProdutos() {
  carregando.value = true;
  erro.value = '';

  try {
    produtos.value = await getProducts();
  } catch {
    erro.value = 'Não foi possivel carregar os produtos.';
  } finally {
    carregando.value = false;
  }
}

async function salvarProduto(produto) {
  salvando.value = true;
  erro.value = '';

  try {
    if (produtoEmEdicao.value) {
      const produtoAtualizado = {
        ...produtoEmEdicao.value,
        ...produto
      };

      await updateProduct(produtoAtualizado);
      await carregarProdutos();
      produtoEmEdicao.value = null;
      return;
    }

    const produtoCriado = await createProduct(productWithDefaults(produto));
    produtos.value = [produtoCriado, ...produtos.value];
  } catch {
    erro.value = 'Não foi possível salvar o produto.';
  } finally {
    salvando.value = false;
  }
}

function productWithDefaults(produto) {
  return {
    custo: 0,
    ...produto
  };
}

function editarProduto(produto) {
  produtoEmEdicao.value = produto;
}

function abrirModalExcluir(produto) {

  produtoSelecionado.value = produto;
  mostrarModalExcluir.value = true;
  
}

async function confirmarExclusao() {

  const produto = produtoSelecionado.value;

  erro.value = '';

  try {

    await deleteProduct(produto.id);

    produtos.value = produtos.value.filter(
      (item) => item.id !== produto.id
    );

    mostrarModalExcluir.value = false;

  } catch {

    erro.value = 'Não foi possível excluir o produto.'

  }

}


onMounted(carregarProdutos);
</script>

<template>
  <section>
    <PageHeader
      title="Produtos"
      subtitle="Cadastro integrado com a API"
    />

    <p v-if="produtoEmEdicao" class="edit-message">
      Editando {{ produtoEmEdicao.nome }}. Envie o formulario para atualizar.
    </p>

    <ProdutoForm
      :loading="salvando"
      :initial-product="produtoEmEdicao"
      @submit="salvarProduto"
      @cancel="produtoEmEdicao = null"
    />

    <section class="card products-toolbar">
      <label class="search-field" for="buscaProduto">
        <span>Buscar produto</span>
        <input
          id="buscaProduto"
          v-model="busca"
          type="search"
          placeholder="Digite nome ou código"
        />
      </label>
    </section>

    <p v-if="carregando" class="state-message">
      Carregando produtos...
    </p>

    <p v-else-if="erro" class="state-message error">
      {{ erro }}
    </p>

    <p v-else-if="!produtosFiltrados.length" class="state-message">
      Nenhum produto encontrado.
    </p>

    <ProdutosTable
      v-else
      :produtos="produtosFiltrados"
      @edit="editarProduto"
      @delete="abrirModalExcluir"
    />

    <ConfirmModal
      :show="mostrarModalExcluir"
      @cancel="mostrarModalExcluir = false"
      @confirm="confirmarExclusao"
    />
    
  </section>
</template>

<style scoped>
.products-toolbar,
.edit-message {
  margin-bottom: 18px;
}

.search-field {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.search-field span {
  color: #475569;
  font-size: 13px;
  font-weight: 700;
}

.search-field input {
  max-width: 420px;
}

.edit-message,
.state-message {
  margin-top: 0;
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
</style>
