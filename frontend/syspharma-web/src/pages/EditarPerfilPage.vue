<script setup>
import { reactive, ref, onMounted, computed } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '../stores/authStore';
import { updateUser } from '../api/users';
import FormField from '../components/FormField.vue';
import PageHeader from '../components/PageHeader.vue';

const router = useRouter();
const auth = useAuthStore();

const salvando = ref(false);
const erro = ref('');
const sucesso = ref(false);
const gravatarUrl = ref('');

const form = reactive({
  nome: '',
  email: '',
  telefone: ''
});

const erros = reactive({
  nome: '',
  email: ''
});

// Gera a URL do Gravatar com SHA-256
async function gerarGravatar(email) {
  if (!email) return;

  const encoded = new TextEncoder().encode(email.trim().toLowerCase());
  const hashBuffer = await crypto.subtle.digest('SHA-256', encoded);
  const hex = Array.from(new Uint8Array(hashBuffer))
    .map((b) => b.toString(16).padStart(2, '0'))
    .join('');

  gravatarUrl.value = `https://www.gravatar.com/avatar/${hex}?d=mp&s=128`;
}

// Inicial do nome para fallback do avatar
const inicialNome = computed(() => {
  return form.nome?.charAt(0).toUpperCase() ?? '?';
});

function validarFormulario() {
  erros.nome = form.nome.trim() ? '' : 'Informe seu nome completo.';
  erros.email = form.email.trim() ? '' : 'Informe seu email.';

  return Object.values(erros).every((e) => e === '');
}

async function handleSubmit() {
  if (!validarFormulario()) return;

  salvando.value = true;
  erro.value = '';
  sucesso.value = false;

  try {
    await updateUser({
      id: auth.user.id,
      nome: form.nome.trim(),
      email: form.email.trim(),
      telefone: form.telefone.trim() || null,
      ativo: true
    });

    // Atualiza o nome no store para refletir na sidebar imediatamente
    auth.user.name = form.nome.trim();

    sucesso.value = true;

    // Regera o Gravatar caso o email tenha mudado
    await gerarGravatar(form.email.trim());

    setTimeout(() => {
      sucesso.value = false;
    }, 3000);
  } catch (e) {
    erro.value =
      e.response?.data?.detail ??
      e.response?.data?.errors?.Email?.[0] ??
      'Não foi possível salvar as alterações.';
  } finally {
    salvando.value = false;
  }
}

onMounted(async () => {
  // Preenche o formulário com os dados atuais do usuário logado
  if (auth.user) {
    form.nome = auth.user.name ?? '';
    form.email = auth.user.email ?? '';
    form.telefone = auth.user.phone ?? '';

    await gerarGravatar(auth.user.email);
  }
});
</script>

<template>
  <section>
    <PageHeader
      title="Editar Perfil"
      subtitle="Atualize suas informações pessoais"
    />

    <div class="page-content">
      <div class="card perfil-card">

        <!-- Avatar centralizado -->
        <div class="avatar-section">
          <div class="avatar-wrapper">
            <div class="avatar">
              <img
                v-if="gravatarUrl"
                :src="gravatarUrl"
                :alt="form.nome"
                class="avatar-img"
                @error="gravatarUrl = ''"
              />
              <span v-else>{{ inicialNome }}</span>
            </div>
          </div>

          <div class="avatar-info">
            <span class="avatar-nome">{{ form.nome }}</span>
            <a
              class="avatar-gravatar-link"
              href="https://gravatar.com"
              target="_blank"
              rel="noopener noreferrer"
            >
              Alterar foto via Gravatar
            </a>
          </div>
        </div>

        <div class="divider" />

        <!-- Mensagens de feedback -->
        <p v-if="sucesso" class="state-message success">
          Perfil atualizado com sucesso!
        </p>

        <p v-if="erro" class="state-message error">
          {{ erro }}
        </p>

        <!-- Formulário -->
        <form class="form" @submit.prevent="handleSubmit">
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

          <FormField
            id="telefone"
            v-model="form.telefone"
            label="Telefone"
          />

          <div class="form-actions">
            <button type="submit" :disabled="salvando">
              {{ salvando ? 'Salvando...' : 'Salvar alterações' }}
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

.perfil-card {
  width: min(100%, 520px);
}

/* Avatar */
.avatar-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 12px;
  margin-bottom: 4px;
}

.avatar-wrapper {
  position: relative;
}

.avatar {
  width: 96px;
  height: 96px;
  border-radius: 50%;
  background: #1f8a70;
  color: #fff;
  font-size: 36px;
  font-weight: 700;
  display: flex;
  align-items: center;
  justify-content: center;
  overflow: hidden;
  border: 3px solid #e1e7ef;
}

.avatar-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.avatar-info {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 4px;
}

.avatar-nome {
  font-size: 18px;
  font-weight: 700;
  color: #172033;
}

.avatar-gravatar-link {
  font-size: 12px;
  color: #1f8a70;
  text-decoration: underline;
}

.avatar-gravatar-link:hover {
  color: #19745f;
}

.divider {
  height: 1px;
  background: #e1e7ef;
  margin: 20px 0;
}

/* Formulário */
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

/* Feedback */
.state-message {
  margin: 0 0 16px;
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