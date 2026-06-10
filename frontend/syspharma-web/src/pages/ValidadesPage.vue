<script setup>
import { onMounted, ref } from 'vue';
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

// ─── carregamento inicial ──────────────────────────────────────────

// Carrega os lotes vencendo para a tabela
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

// Carrega produtos e fornecedores para os dropdowns do formulário
async function carregarDadosFormulario() {
  try {
    const [listaProdutos, listaFornecedores] = await Promise.all([
      getProducts(),
      getSuppliers()
    ]);

    produtos.value = listaProdutos;
    fornecedores.value = listaFornecedores;
  } catch {
    // Se falhar, os dropdowns ficam vazios mas não bloqueamos a página
    console.error('Erro ao carregar dados do formulário.');
  }
}

// Ao montar a página, carregamos tudo em paralelo
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

    // Após salvar, recarregamos a tabela para refletir o novo lote
    await carregarLotes();

    sucessoSalvamento.value = true;

    // Remove a mensagem de sucesso após 4 segundos
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

    <!-- Estados da tabela -->
    <p v-if="carregando" class="state-message">
      Carregando lotes...
    </p>

    <p v-else-if="erroCarregamento" class="state-message error">
      {{ erroCarregamento }}
    </p>

    <p v-else-if="!lotes.length" class="state-message">
      Nenhum lote próximo do vencimento encontrado.
    </p>

    <LotesTable v-else :lotes="lotes" />

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
</style>