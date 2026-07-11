<script setup>
import { onMounted, ref, computed } from 'vue';
import { getExpiringBatches, createBatch } from '../api/batches';
import { getProducts } from '../api/products';
import { getSuppliers } from '../api/suppliers';
import PageHeader from '../components/PageHeader.vue';
import LoteForm from '../components/LoteForm.vue';
import LotesTable from '../components/LotesTable.vue';

// ─── dados da tabela ───────────────────────────────────────────────
const lotes = ref([]);
const carregando = ref(false);
const erroCarregamento = ref('');

// ─── dados do formulário ───────────────────────────────────────────
const produtos = ref([]);
const fornecedores = ref([]);
const salvando = ref(false);
const erroSalvamento = ref('');
const sucessoSalvamento = ref(false);

// ─── busca e filtros ───────────────────────────────────────────────
const busca = ref('');
const filtroAtivo = ref('todos');

const filtros = [
  { valor: 'todos',    label: 'Todos' },
  { valor: 'vencendo', label: 'À vencer',              descricao: 'Até 30 dias' },
  { valor: 'atencao',  label: 'Próximos à validade',   descricao: '31 a 90 dias' },
  { valor: 'ok',       label: 'Longe do vencimento',   descricao: 'Acima de 90 dias' }
];

// Aplica busca e filtro de cor sobre os lotes carregados
const lotesFiltrados = computed(() => {
  let resultado = lotes.value;

  // Filtro de busca por nome do produto ou código do lote
  const termo = busca.value.trim().toLowerCase();
  if (termo) {
    resultado = resultado.filter((lote) => {
      const nomeProduto = (lote.description ?? lote.productInternalCode ?? '').toLowerCase();
      const codigoLote = (lote.batchCode ?? '').toLowerCase();
      const codigoProduto = (lote.productInternalCode ?? '').toLowerCase();

      return (
        nomeProduto.includes(termo) ||
        codigoLote.includes(termo) ||
        codigoProduto.includes(termo)
      );
    });
  }

  // Filtro de cor com base nos dias para vencer
  if (filtroAtivo.value !== 'todos') {
    resultado = resultado.filter((lote) => {
      const dias = lote.expiresInDays;

      if (filtroAtivo.value === 'vencendo') return dias <= 30;
      if (filtroAtivo.value === 'atencao')  return dias > 30 && dias <= 90;
      if (filtroAtivo.value === 'ok')       return dias > 90;

      return true;
    });
  }

  return resultado;
});

// Conta lotes por categoria para exibir nos botões de filtro
const contagens = computed(() => ({
  todos:    lotes.value.length,
  vencendo: lotes.value.filter((l) => l.expiresInDays <= 30).length,
  atencao:  lotes.value.filter((l) => l.expiresInDays > 30 && l.expiresInDays <= 90).length,
  ok:       lotes.value.filter((l) => l.expiresInDays > 90).length
}));

// ─── carregamento inicial ──────────────────────────────────────────
async function carregarLotes() {
  carregando.value = true;
  erroCarregamento.value = '';

  try {
    lotes.value = await getExpiringBatches();
  } catch {
    erroCarregamento.value = 'Não foi possível carregar os lotes.';
  } finally {
    carregando.value = false;
  }
}

async function carregarDadosFormulario() {
  try {
    const [listaProdutos, listaFornecedores] = await Promise.all([
      getProducts(),
      getSuppliers()
    ]);

    produtos.value = listaProdutos;
    fornecedores.value = listaFornecedores;
  } catch {
    console.error('Erro ao carregar dados do formulário.');
  }
}

onMounted(() => {
  carregarLotes();
  carregarDadosFormulario();
});

// ─── ações ────────────────────────────────────────────────────────
async function salvarLote(dadosLote) {
  salvando.value = true;
  erroSalvamento.value = '';
  sucessoSalvamento.value = false;

  try {
    await createBatch(dadosLote);
    await carregarLotes();

    sucessoSalvamento.value = true;

    setTimeout(() => {
      sucessoSalvamento.value = false;
    }, 4000);
  } catch {
    erroSalvamento.value = 'Não foi possível cadastrar o lote. Verifique os dados e tente novamente.';
  } finally {
    salvando.value = false;
  }
}
</script>

<template>
  <section>
    <PageHeader
      title="Validades"
      subtitle="Cadastro de lotes e controle de vencimentos"
    />

    <!-- Formulário de cadastro -->
    <LoteForm
      :produtos="produtos"
      :fornecedores="fornecedores"
      :loading="salvando"
      @submit="salvarLote"
    />

    <!-- Feedback de salvamento -->
    <p v-if="erroSalvamento" class="state-message error">
      {{ erroSalvamento }}
    </p>

    <p v-if="sucessoSalvamento" class="state-message success">
      Lote cadastrado com sucesso! A tabela foi atualizada.
    </p>

    <!-- Separador visual -->
    <div class="section-divider">
      <span>Lotes próximos do vencimento</span>
    </div>

    <!-- Barra de busca e filtros -->
    <div class="card toolbar">
      <label class="search-field" for="buscaLote">
        <span>Buscar lote</span>
        <input
          id="buscaLote"
          v-model="busca"
          type="search"
          placeholder="Digite o nome do produto ou código do lote"
        />
      </label>

      <div class="filtros">
        <span class="filtros-label">Filtrar por situação:</span>
        <div class="filtros-botoes">
          <button
            v-for="filtro in filtros"
            :key="filtro.valor"
            :class="['filtro-btn', filtro.valor, { ativo: filtroAtivo === filtro.valor }]"
            type="button"
            @click="filtroAtivo = filtro.valor"
          >
            {{ filtro.label }}
            <span class="filtro-count">{{ contagens[filtro.valor] }}</span>
          </button>
        </div>
      </div>
    </div>

    <!-- Estados da tabela -->
    <p v-if="carregando" class="state-message">
      Carregando lotes...
    </p>

    <p v-else-if="erroCarregamento" class="state-message error">
      {{ erroCarregamento }}
    </p>

    <p v-else-if="!lotesFiltrados.length" class="state-message">
      Nenhum lote encontrado para os filtros aplicados.
    </p>

    <LotesTable v-else :lotes="lotesFiltrados" />

  </section>
</template>

<style scoped>
.section-divider {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 18px;
}

.section-divider::before,
.section-divider::after {
  content: '';
  flex: 1;
  height: 1px;
  background: #e1e7ef;
}

.section-divider span {
  color: #64748b;
  font-size: 13px;
  font-weight: 700;
  white-space: nowrap;
}

/* Toolbar */
.toolbar {
  display: flex;
  flex-direction: column;
  gap: 16px;
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
  max-width: 480px;
}

/* Filtros */
.filtros {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.filtros-label {
  color: #475569;
  font-size: 13px;
  font-weight: 700;
}

.filtros-botoes {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
}

.filtro-btn {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 14px;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  border: 2px solid transparent;
  transition: all 0.15s;
  background: #f1f5f9;
  color: #475569;
}

.filtro-btn:hover {
  background: #e2e8f0;
}

/* Filtro ativo — todos */
.filtro-btn.todos.ativo {
  background: #172033;
  color: #fff;
  border-color: #172033;
}

/* Filtro ativo — vencendo (vermelho) */
.filtro-btn.vencendo {
  color: #991b1b;
}

.filtro-btn.vencendo.ativo {
  background: #fef2f2;
  border-color: #dc2626;
  color: #991b1b;
}

/* Filtro ativo — atenção (amarelo) */
.filtro-btn.atencao {
  color: #92400e;
}

.filtro-btn.atencao.ativo {
  background: #fefce8;
  border-color: #eab308;
  color: #92400e;
}

/* Filtro ativo — ok (verde) */
.filtro-btn.ok {
  color: #166534;
}

.filtro-btn.ok.ativo {
  background: #f0fdf4;
  border-color: #22c55e;
  color: #166534;
}

/* Contador dentro do botão */
.filtro-count {
  background: rgba(0, 0, 0, 0.08);
  border-radius: 999px;
  padding: 1px 7px;
  font-size: 11px;
  font-weight: 700;
}

/* Feedback */
.state-message {
  margin: 0 0 18px;
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

.state-message.success {
  border-color: #bbf7d0;
  background: #f0fdf4;
  color: #166534;
}

@media (max-width: 800px) {
  .filtros-botoes {
    flex-direction: column;
  }

  .filtro-btn {
    width: 100%;
    justify-content: space-between;
  }
}
</style>