<script setup>
import { reactive, watch } from 'vue';
import FormField from './FormField.vue';

const props = defineProps({
  produtos: {
    type: Array,
    default: () => []
  },
  fornecedores: {
    type: Array,
    default: () => []
  },
  loading: {
    type: Boolean,
    default: false
  }
});

const emit = defineEmits(['submit']);

// Estado interno do formulário
/* guarda o estado de todos os campos do formulário. 
Usamos reactive em vez de vários ref separados porque os campos estão todos relacionados ao mesmo objeto*/
const lote = reactive({
  idProduto: '',
  idFornecedor: '',
  codigoLote: '',
  dataValidade: '',
  dataFabricacao: '',
  quantidade: '',
  custoUnitario: ''
});

// Estado dos erros de validação
const erros = reactive({
  idProduto: '',
  codigoLote: '',
  dataValidade: '',
  quantidade: '',
  custoUnitario: ''
});

// Limpa o formulário após salvar
function limparFormulario() {
  lote.idProduto = '';
  lote.idFornecedor = '';
  lote.codigoLote = '';
  lote.dataValidade = '';
  lote.dataFabricacao = '';
  lote.quantidade = '';
  lote.custoUnitario = '';

  erros.idProduto = '';
  erros.codigoLote = '';
  erros.dataValidade = '';
  erros.quantidade = '';
  erros.custoUnitario = '';
}

// Valida os campos obrigatórios e retorna true se tudo estiver ok
function validarFormulario() {
  erros.idProduto = lote.idProduto ? '' : 'Selecione um produto.';
  erros.codigoLote = lote.codigoLote.trim() ? '' : 'Informe o código do lote.';
  erros.dataValidade = lote.dataValidade ? '' : 'Informe a data de validade.';
  erros.quantidade = Number(lote.quantidade) > 0 ? '' : 'Informe uma quantidade maior que zero.';
  erros.custoUnitario = Number(lote.custoUnitario) >= 0 ? '' : 'Informe um custo válido.';

  return Object.values(erros).every((erro) => erro === '');
}

function enviarFormulario() {
  if (!validarFormulario()) return;

  emit('submit', {
    idProduto: Number(lote.idProduto),
    idFornecedor: lote.idFornecedor ? Number(lote.idFornecedor) : null,
    codigoLote: lote.codigoLote.trim(),
    dataValidade: lote.dataValidade,
    dataFabricacao: lote.dataFabricacao || null,
    quantidade: Number(lote.quantidade),
    custoUnitario: Number(lote.custoUnitario)
  });

  limparFormulario();
}
</script>

<template>
  <form class="card form" @submit.prevent="enviarFormulario">

    <!-- Linha 1: produto e fornecedor -->
    <div class="form-row">
      <label class="form-field" for="idProduto">
        <span>Produto *</span>
        <select
          id="idProduto"
          v-model="lote.idProduto"
          :class="{ invalid: erros.idProduto }"
        >
          <option value="">Selecione um produto</option>
          <option
            v-for="produto in produtos"
            :key="produto.id"
            :value="produto.id"
          >
            {{ produto.nome }} — {{ produto.codigoBarras }}
          </option>
        </select>
        <small v-if="erros.idProduto">{{ erros.idProduto }}</small>
      </label>

      <label class="form-field" for="idFornecedor">
        <span>Fornecedor</span>
        <select id="idFornecedor" v-model="lote.idFornecedor">
          <option value="">Sem fornecedor</option>
          <option
            v-for="fornecedor in fornecedores"
            :key="fornecedor.id"
            :value="fornecedor.id"
          >
            {{ fornecedor.nomeFantasia || fornecedor.nome }}
          </option>
        </select>
      </label>
    </div>

    <!-- Linha 2: código, datas, quantidade e custo -->
    <div class="form-row">
      <FormField
        id="codigoLote"
        v-model="lote.codigoLote"
        label="Código do lote *"
        :error="erros.codigoLote"
      />

      <FormField
        id="dataValidade"
        v-model="lote.dataValidade"
        label="Data de validade *"
        type="date"
        :error="erros.dataValidade"
      />

      <FormField
        id="dataFabricacao"
        v-model="lote.dataFabricacao"
        label="Data de fabricação"
        type="date"
      />

      <FormField
        id="quantidade"
        v-model="lote.quantidade"
        label="Quantidade *"
        type="number"
        :error="erros.quantidade"
      />

      <FormField
        id="custoUnitario"
        v-model="lote.custoUnitario"
        label="Custo unitário (R$) *"
        type="number"
        :error="erros.custoUnitario"
      />
    </div>

    <button type="submit" :disabled="loading">
      {{ loading ? 'Salvando...' : 'Cadastrar lote' }}
    </button>

  </form>
</template>

<style scoped>
.form {
  display: flex;
  flex-direction: column;
  gap: 14px;
  margin-bottom: 24px;
}

.form-row {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
  gap: 12px;
  align-items: end;
}

/* Reutilizamos o estilo do FormField, mas o select precisa do mesmo visual do input */
select {
  width: 100%;
  border: 1px solid #cfd8e3;
  border-radius: 8px;
  padding: 11px 12px;
  outline: none;
  background: #fff;
  font-size: 14px;
  color: #172033;
}

select:focus {
  border-color: #1f8a70;
}

select.invalid {
  border-color: #dc2626;
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

.form-field small {
  color: #dc2626;
  font-size: 12px;
  font-weight: 600;
}

button {
  align-self: flex-start;
  min-width: 160px;
}

button:disabled {
  background: #94a3b8;
  cursor: not-allowed;
}
</style>