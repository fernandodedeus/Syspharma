<script setup>
import { computed, onMounted, ref } from 'vue';
import { api } from '../api/http'; // Ajuste o caminho conforme necessário
import PageHeader from '../components/PageHeader.vue'; // Componente de cabeçalho reutilizável
import ProdutoForm from '../components/ProdutoForm.vue';
import ProdutosTable from '../components/ProdutosTable.vue';
import { produtosMockados } from '../mocks/produtosMock'; // Dados mockados para testar a interface, na prática esses dados viriam da API

const produtos = ref([]);



// DADOS MOCKADOS, PARA TESTAR A INTERFACE. NA PRÁTICA, ESSES DADOS VIRIAM DA API, ASSIM COMO O CÓDIGOS DE EDIÇÃO E EXCLUSÃO DE PRODUTOS.

const busca = ref('');
const carregando = ref(false);
const erro = ref('');

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

// ----------------------------------------------


async function carregarProdutos() {
  carregando.value = true;
  erro.value = '';

  try {
    await new Promise((resolve) => setTimeout(resolve, 500));
    produtos.value = produtosMockados;
  } catch {
    erro.value = 'Não foi possível carregar os produtos.';
  } finally {
    carregando.value = false;
  }
}

// -------------------------------------------------------

/* async function carregarProdutos() {
  const response = await api.get('/produtos'); // Ajuste o endpoint conforme necessário
  produtos.value = response.data;
} */

/* async function salvarProduto() {

  await api.post('/produtos', novoProduto.value); // Ajuste o endpoint conforme necessário
  await carregarProdutos(); // Recarrega a lista de produtos após salvar um novo produto

} */ 

// -------------------------------------------
async function salvarProduto(produto) {
  const novoProduto = {
    id: Date.now(),
    ...produto
  };

  produtos.value = [novoProduto, ...produtos.value];
}
// --------------------------------------------


function editarProduto(produto) { 
  console.log('Editar produto:', produto); // trocar para api.put quando tiver o endpoint de edição pronto
}

function excluirProduto(produto) {
  console.log('Excluir produto:', produto);
}

onMounted(carregarProdutos); // Carrega a lista de produtos quando o componente for montado
</script>

<template>


  <section>
    <PageHeader
      title="Produtos"
      subtitle="Cadastro inicial para testar a interface"
    /> <!-- Uso do componente de cabeçalho, passando o título e a descrição como props -->


    <ProdutoForm @submit="salvarProduto" /> <!-- Uso do componente de formulário, ouvindo o evento 'submit' para salvar o produto -->



      <!-- Área de busca, para filtrar os produtos por nome ou código de barras -->

              <section class="card products-toolbar">
                <label class="search-field" for="buscaProduto">
                  <span>Buscar produto</span>
                  <input
                    id="buscaProduto"
                    v-model="busca"
                    type="search"
                    placeholder="Digite nome ou código de barras"
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
                        @delete="excluirProduto"
                      />
                      
                </section>

              </template>

<style scoped>

.products-toolbar {
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

.state-message {
  margin: 0;
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