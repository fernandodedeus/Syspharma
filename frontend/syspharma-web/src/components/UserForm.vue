<script setup>
import { reactive, watch } from 'vue';
import FormField from './FormField.vue';

const props = defineProps({
  lojas: {
    type: Array,
    default: () => []
  },
  initialUser: {
    type: Object,
    default: null
  },
  loading: {
    type: Boolean,
    default: false
  }
});

const emit = defineEmits(['submit', 'cancel']);

const form = reactive({
  idLoja: '',
  nome: '',
  email: '',
  cpf: '',
  telefone: '',
  ativo: true,
  senha: ''
});

const erros = reactive({
  idLoja: '',
  nome: '',
  email: '',
  cpf: '',
  senha: ''
});

// Quando recebe um usuário para edição, preenche o formulário
watch(
  () => props.initialUser,
  (user) => {
    if (!user) {
      limparFormulario();
      return;
    }

    form.idLoja = user.idLoja ?? '';
    form.nome = user.nome;
    form.email = user.email;
    form.cpf = user.cpf ?? '';
    form.telefone = user.telefone ?? '';
    form.ativo = user.ativo ?? true;
    form.senha = ''; // senha nunca vem preenchida na edição
  },
  { immediate: true }
);

function limparFormulario() {
  form.idLoja = '';
  form.nome = '';
  form.email = '';
  form.cpf = '';
  form.telefone = '';
  form.ativo = true;
  form.senha = '';

  erros.idLoja = '';
  erros.nome = '';
  erros.email = '';
  erros.cpf = '';
  erros.senha = '';
}

function validarFormulario() {
  erros.nome = form.nome.trim() ? '' : 'Informe o nome completo.';
  erros.email = form.email.trim() ? '' : 'Informe o email.';
  erros.cpf = form.cpf.trim() ? '' : 'Informe o CPF.';

  // Senha só é obrigatória no cadastro, não na edição
  if (!props.initialUser) {
    erros.senha = form.senha.length >= 6 ? '' : 'A senha precisa ter no mínimo 6 caracteres.';
  }

  return Object.values(erros).every((erro) => erro === '');
}

function enviarFormulario() {
  if (!validarFormulario()) return;

  emit('submit', {
    idLoja: form.idLoja ? Number(form.idLoja) : null,
    nome: form.nome.trim(),
    email: form.email.trim(),
    cpf: form.cpf.trim(),
    telefone: form.telefone.trim() || null,
    ativo: form.ativo,
    senha: form.senha || null
  });

  if (!props.initialUser) {
    limparFormulario();
  }
}

function cancelarEdicao() {
  emit('cancel');
  limparFormulario();
}
</script>

<template>
  <form class="card form" @submit.prevent="enviarFormulario">

    <!-- Linha 1: loja e situação -->
    <div class="form-row">
      <label class="form-field" for="idLoja">
        <span>Loja vinculada</span>
        <select id="idLoja" v-model="form.idLoja">
          <option value="">Sem loja vinculada</option>
          <option
            v-for="loja in lojas"
            :key="loja.id"
            :value="loja.id"
          >
            {{ loja.nomeFantasia || loja.razaoSocial }}
          </option>
        </select>
      </label>

      <label class="form-field" for="ativo">
        <span>Situação</span>
        <select id="ativo" v-model="form.ativo">
          <option :value="true">Ativo</option>
          <option :value="false">Inativo</option>
        </select>
      </label>
    </div>

    <!-- Linha 2: nome e email -->
    <div class="form-row">
      <FormField
        id="nome"
        v-model="form.nome"
        label="Nome completo *"
        :error="erros.nome"
      />

      <FormField
        id="email"
        v-model="form.email"
        label="Email *"
        type="email"
        :error="erros.email"
      />
    </div>

    <!-- Linha 3: cpf e telefone -->
    <div class="form-row">
      <FormField
        id="cpf"
        v-model="form.cpf"
        label="CPF *"
        :error="erros.cpf"
      />

      <FormField
        id="telefone"
        v-model="form.telefone"
        label="Telefone"
      />
    </div>

    <!-- Senha: só aparece no cadastro, não na edição -->
    <div v-if="!initialUser" class="form-row">
      <FormField
        id="senha"
        v-model="form.senha"
        label="Senha padrão *"
        type="password"
        :error="erros.senha"
      />
    </div>

    <!-- Ações -->
    <div class="form-actions">
      <button type="submit" :disabled="loading">
        {{ loading ? 'Salvando...' : initialUser ? 'Atualizar' : 'Cadastrar funcionário' }}
      </button>

      <button
        v-if="initialUser"
        class="button-secondary"
        type="button"
        @click="cancelarEdicao"
      >
        Cancelar
      </button>
    </div>

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
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 12px;
  align-items: end;
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

.form-actions {
  display: flex;
  gap: 10px;
}

.button-secondary {
  background: #e2e8f0;
  color: #172033;
}

.button-secondary:hover {
  background: #cbd5e1;
}

button:disabled {
  background: #94a3b8;
  cursor: not-allowed;
}

@media (max-width: 800px) {
  .form-row {
    grid-template-columns: 1fr;
  }
}
</style>