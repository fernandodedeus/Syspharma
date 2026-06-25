<script setup>
import { reactive, ref } from 'vue';
import { useRouter } from 'vue-router';
import { switchPassword } from '../api/users';
import FormField from '../components/FormField.vue';
import PageHeader from '../components/PageHeader.vue';

const router = useRouter();

const form = reactive({
  senhaAntiga: '',
  novaSenha: '',
  confirmarSenha: ''
});

const erros = reactive({
  senhaAntiga: '',
  novaSenha: '',
  confirmarSenha: ''
});

const salvando = ref(false);
const erro = ref('');
const sucesso = ref(false);

function validarFormulario() {
  erros.senhaAntiga = form.senhaAntiga.length >= 6
    ? ''
    : 'Informe sua senha atual.';

  erros.novaSenha = form.novaSenha.length >= 6
    ? ''
    : 'A nova senha precisa ter no mínimo 6 caracteres.';

  erros.confirmarSenha = form.novaSenha === form.confirmarSenha
    ? ''
    : 'As senhas não conferem.';

  return Object.values(erros).every((e) => e === '');
}

async function handleSubmit() {
  if (!validarFormulario()) return;

  salvando.value = true;
  erro.value = '';
  sucesso.value = false;

  try {
    await switchPassword(form.senhaAntiga, form.novaSenha);
    sucesso.value = true;

    // Limpa o formulário após sucesso
    form.senhaAntiga = '';
    form.novaSenha = '';
    form.confirmarSenha = '';

    // Redireciona para o dashboard após 2 segundos
    setTimeout(() => {
      router.push('/');
    }, 2000);
  } catch (e) {
    erro.value =
      e.response?.data?.detail ??
      'Não foi possível trocar a senha. Verifique sua senha atual.';
  } finally {
    salvando.value = false;
  }
}
</script>

<template>
  <section>
    <PageHeader
      title="Trocar Senha"
      subtitle="Atualize sua senha de acesso"
    />

    <div class="page-content">
      <div class="card form-card">

        <p v-if="sucesso" class="state-message success">
          Senha alterada com sucesso! Redirecionando...
        </p>

        <p v-if="erro" class="state-message error">
          {{ erro }}
        </p>

        <form
          v-if="!sucesso"
          class="form"
          @submit.prevent="handleSubmit"
        >
          <FormField
            id="senhaAntiga"
            v-model="form.senhaAntiga"
            label="Senha atual *"
            type="password"
            :error="erros.senhaAntiga"
          />

          <FormField
            id="novaSenha"
            v-model="form.novaSenha"
            label="Nova senha *"
            type="password"
            :error="erros.novaSenha"
          />

          <FormField
            id="confirmarSenha"
            v-model="form.confirmarSenha"
            label="Confirmar nova senha *"
            type="password"
            :error="erros.confirmarSenha"
          />

          <div class="form-actions">
            <button type="submit" :disabled="salvando">
              {{ salvando ? 'Salvando...' : 'Trocar senha' }}
            </button>

            <button
              class="button-secondary"
              type="button"
              @click="router.push('/')"
            >
              Cancelar
            </button>
          </div>
        </form>

      </div>
    </div>
  </section>
</template>

<style scoped>
.page-content {
  display: flex;
  justify-content: center;
}

.form-card {
  width: min(100%, 480px);
}

.form {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.form-actions {
  display: flex;
  gap: 10px;
  margin-top: 8px;
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

.state-message {
  margin: 0 0 18px;
  border-radius: 8px;
  padding: 16px;
  font-size: 14px;
}

.state-message.success {
  border: 1px solid #bbf7d0;
  background: #f0fdf4;
  color: #166534;
}

.state-message.error {
  border: 1px solid #fecaca;
  background: #fef2f2;
  color: #991b1b;
}
</style>