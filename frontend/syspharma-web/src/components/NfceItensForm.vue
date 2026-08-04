<script setup>
import { ref, computed } from 'vue';
import { getProducts } from '../api/products';

const emit = defineEmits(['update:itens']);

const props = defineProps({
  itens: {
    type: Array,
    default: () => []
  }
});

const busca = ref('');
const quantidade = ref(1);
const produtoSelecionado = ref(null);
const resultadosBusca = ref([]);
const buscando = ref(false);
const erroBusca = ref('');

// Debounce para não buscar a cada tecla
let debounceTimer = null;

function onBuscaInput() {
  clearTimeout(debounceTimer);
  produtoSelecionado.value = null;
  erroBusca.value = '';

  if (!busca.value.trim()) {
    resultadosBusca.value = [];
    return;
  }

  debounceTimer = setTimeout(async () => {
    await buscarProdutos();
  }, 350);
}

async function buscarProdutos() {
  buscando.value = true;

  try {
    const todos = await getProducts();
    const termo = busca.value.trim().toLowerCase();

    resultadosBusca.value = todos.filter((p) =>
      p.nome.toLowerCase().includes(termo) ||
      p.codigoBarras.toLowerCase().includes(termo)
    ).slice(0, 6); // limita a 6 resultados
  } catch {
    erroBusca.value = 'Erro ao buscar produtos.';
  } finally {
    buscando.value = false;
  }
}

function selecionarProduto(produto) {
  produtoSelecionado.value = produto;
  busca.value = produto.nome;
  resultadosBusca.value = [];
}

function adicionarItem() {
  if (!produtoSelecionado.value) return;

  const qtd = Number(quantidade.value);
  if (qtd <= 0) return;

  // Verifica se o produto já está na lista
  const itemExistente = props.itens.find(
    (i) => i.idProduto === produtoSelecionado.value.id
  );

  let novosItens;

  if (itemExistente) {
    // Atualiza a quantidade se já existe
    novosItens = props.itens.map((i) =>
      i.idProduto === produtoSelecionado.value.id
        ? { ...i, quantidade: i.quantidade + qtd, total: (i.quantidade + qtd) * i.precoUnitario }
        : i
    );
  } else {
    const novoItem = {
      idProduto: produtoSelecionado.value.id,
      nome: produtoSelecionado.value.nome,
      codigoBarras: produtoSelecionado.value.codigoBarras,
      precoUnitario: produtoSelecionado.value.precoVenda,
      quantidade: qtd,
      total: qtd * produtoSelecionado.value.precoVenda
    };
    novosItens = [...props.itens, novoItem];
  }

  emit('update:itens', novosItens);

  // Limpa os campos após adicionar
  busca.value = '';
  quantidade.value = 1;
  produtoSelecionado.value = null;
  resultadosBusca.value = [];
}

function removerItem(idProduto) {
  emit('update:itens', props.itens.filter((i) => i.idProduto !== idProduto));
}

function formatarPreco(valor) {
  return Number(valor ?? 0).toLocaleString('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  });
}
</script>

<template>
  <div class="itens-form">
    <h3 class="section-title">Itens</h3>

    <!-- Busca de produto -->
    <div class="busca-wrapper">
      <div class="busca-row">
        <div class="busca-field">
          <label for="buscaProduto" class="field-label">Produto</label>
          <input
            id="buscaProduto"
            v-model="busca"
            type="search"
            placeholder="Nome ou código do produto"
            autocomplete="off"
            @input="onBuscaInput"
          />

          <!-- Dropdown de resultados -->
          <div v-if="resultadosBusca.length" class="busca-dropdown">
            <button
              v-for="produto in resultadosBusca"
              :key="produto.id"
              class="busca-item"
              type="button"
              @click="selecionarProduto(produto)"
            >
              <span class="busca-item-nome">{{ produto.nome }}</span>
              <span class="busca-item-preco">{{ formatarPreco(produto.precoVenda) }}</span>
            </button>
          </div>

          <p v-if="buscando" class="busca-hint">Buscando...</p>
          <p v-if="erroBusca" class="busca-hint error">{{ erroBusca }}</p>
        </div>

        <div class="quantidade-field">
          <label for="quantidade" class="field-label">Qtd.</label>
          <input
            id="quantidade"
            v-model="quantidade"
            type="number"
            min="1"
            step="1"
          />
        </div>

        <button
          class="btn-adicionar"
          type="button"
          :disabled="!produtoSelecionado"
          @click="adicionarItem"
        >
          Adicionar
        </button>
      </div>
    </div>

    <!-- Lista de itens adicionados -->
    <div v-if="itens.length" class="itens-lista">
      <div
        v-for="item in itens"
        :key="item.idProduto"
        class="item-row"
      >
        <div class="item-info">
          <span class="item-nome">{{ item.nome }}</span>
          <span class="item-detalhe">
            {{ item.quantidade }}x {{ formatarPreco(item.precoUnitario) }}
          </span>
        </div>

        <div class="item-acoes">
          <span class="item-total">{{ formatarPreco(item.total) }}</span>
          <button
            class="btn-remover"
            type="button"
            @click="removerItem(item.idProduto)"
          >
            ✕
          </button>
        </div>
      </div>
    </div>

    <p v-else class="itens-vazio">
      Nenhum item adicionado ainda.
    </p>
  </div>
</template>

<style scoped>
.section-title {
  font-size: 14px;
  font-weight: 700;
  color: #172033;
  margin: 0 0 12px;
}

.busca-row {
  display: grid;
  grid-template-columns: 1fr 80px auto;
  gap: 8px;
  align-items: end;
}

.busca-wrapper {
  position: relative;
  margin-bottom: 16px;
}

.busca-field {
  position: relative;
}

.field-label {
  display: block;
  font-size: 12px;
  font-weight: 700;
  color: #475569;
  margin-bottom: 6px;
}

.quantidade-field {
  display: flex;
  flex-direction: column;
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

/* Dropdown de busca */
.busca-dropdown {
  position: absolute;
  top: calc(100% + 4px);
  left: 0;
  right: 0;
  background: #fff;
  border: 1px solid #e1e7ef;
  border-radius: 8px;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.1);
  z-index: 200;
  overflow: hidden;
}

.busca-item {
  width: 100%;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px 14px;
  background: transparent;
  color: #172033;
  font-size: 13px;
  text-align: left;
  border-radius: 0;
  cursor: pointer;
  border-bottom: 1px solid #f1f5f9;
}

.busca-item:last-child {
  border-bottom: none;
}

.busca-item:hover {
  background: #f8fafc;
}

.busca-item-nome {
  font-weight: 500;
}

.busca-item-preco {
  color: #1f8a70;
  font-weight: 700;
  font-size: 12px;
}

.busca-hint {
  font-size: 12px;
  color: #64748b;
  margin: 4px 0 0;
}

.busca-hint.error {
  color: #dc2626;
}

/* Botão adicionar */
.btn-adicionar {
  white-space: nowrap;
  padding: 10px 16px;
  font-size: 13px;
  align-self: end;
}

.btn-adicionar:disabled {
  background: #94a3b8;
  cursor: not-allowed;
}

/* Lista de itens */
.itens-lista {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.item-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: #f8fafc;
  border: 1px solid #e1e7ef;
  border-radius: 8px;
  padding: 10px 14px;
}

.item-info {
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.item-nome {
  font-size: 13px;
  font-weight: 600;
  color: #172033;
}

.item-detalhe {
  font-size: 12px;
  color: #64748b;
}

.item-acoes {
  display: flex;
  align-items: center;
  gap: 12px;
}

.item-total {
  font-size: 14px;
  font-weight: 700;
  color: #172033;
}

.btn-remover {
  background: #fee2e2;
  color: #dc2626;
  border-radius: 6px;
  padding: 4px 8px;
  font-size: 12px;
  line-height: 1;
}

.btn-remover:hover {
  background: #fecaca;
}

.itens-vazio {
  font-size: 13px;
  color: #94a3b8;
  text-align: center;
  padding: 24px 0;
  margin: 0;
}

@media (max-width: 480px) {
  .busca-row {
    grid-template-columns: 1fr 70px;
  }

  .btn-adicionar {
    grid-column: 1 / -1;
    width: 100%;
  }
}
</style>