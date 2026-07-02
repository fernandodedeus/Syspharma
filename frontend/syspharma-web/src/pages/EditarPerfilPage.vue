<script setup>
import { reactive, ref, onMounted, computed } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '../stores/authStore';
import { updateUser, uploadPhoto } from '../api/users';
import FormField from '../components/FormField.vue';
import PageHeader from '../components/PageHeader.vue';

const router = useRouter();
const auth = useAuthStore();

const salvando = ref(false);
const enviandoFoto = ref(false);
const erro = ref('');
const erroFoto = ref('');
const sucesso = ref(false);
const sucessoFoto = ref(false);

const form = reactive({
  nome: '',
  email: '',
  telefone: ''
});

const erros = reactive({
  nome: '',
  email: ''
});

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

    // Atualiza o nome no store imediatamente
    auth.user.name = form.nome.trim();
    auth.user.email = form.email.trim();

    sucesso.value = true;

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

// Abre o seletor de arquivo ao clicar no avatar
function abrirSeletorFoto() {
  document.getElementById('inputFoto').click();
}

// Chamado quando o usuário seleciona uma foto
async function handleFotoSelecionada(event) {
  const file = event.target.files?.[0];
  if (!file) return;

  // Valida o tipo do arquivo
  if (!file.type.startsWith('image/')) {
    erroFoto.value = 'Selecione um arquivo de imagem válido.';
    return;
  }

  // Valida o tamanho máximo de 5MB
  if (file.size > 5 * 1024 * 1024) {
    erroFoto.value = 'A imagem deve ter no máximo 5MB.';
    return;
  }

  enviandoFoto.value = true;
  erroFoto.value = '';
  sucessoFoto.value = false;

  try {
    const path = await uploadPhoto(auth.user.id, file);

    // Atualiza a foto no store para refletir imediatamente
    auth.updatePhoto(path);

    sucessoFoto.value = true;

    setTimeout(() => {
      sucessoFoto.value = false;
    }, 3000);
  } catch {
    erroFoto.value = 'Não foi possível enviar a foto. Tente novamente.';
  } finally {
    enviandoFoto.value = false;

    // Limpa o input para permitir selecionar o mesmo arquivo novamente
    event.target.value = '';
  }
}

onMounted(() => {
  if (auth.user) {
    form.nome = auth.user.name ?? '';
    form.email = auth.user.email ?? '';
    form.telefone = auth.user.phone ?? '';
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

        <!-- Avatar com botão de upload -->
        <div class="avatar-section">
          <div class="avatar-wrapper">
            <div class="avatar">
              <img
                v-if="auth.photoUrl"
                :src="auth.photoUrl"
                :alt="form.nome"
                class="avatar-img"
              />
              <span v-else>{{ inicialNome }}</span>
            </div>

            <!-- Botão de alterar foto sobreposto ao avatar -->
            <button
              class="avatar-edit-btn"
              type="button"
              :disabled="enviandoFoto"
              @click="abrirSeletorFoto"
            >
              <svg
                v-if="!enviandoFoto"
                xmlns="http://www.w3.org/2000/svg"
                width="14"
                height="14"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                stroke-width="2.5"
                stroke-linecap="round"
                stroke-linejoin="round"
              >
                <path d="M23 19a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h4l2-3h6l2 3h4a2 2 0 0 1 2 2z"/>
                <circle cx="12" cy="13" r="4"/>
              </svg>
              <span v-else class="spinner" />
            </button>

            <!-- Input de arquivo oculto -->
            <input
              id="inputFoto"
              type="file"
              accept="image/*"
              class="input-foto-oculto"
              @change="handleFotoSelecionada"
            />
          </div>

          <div class="avatar-info">
            <span class="avatar-nome">{{ form.nome }}</span>
            <span class="avatar-hint">
              {{ enviandoFoto ? 'Enviando foto...' : 'Clique no avatar para alterar a foto' }}
            </span>
          </div>
        </div>

        <!-- Feedback da foto -->
        <p v-if="erroFoto" class="state-message error">
          {{ erroFoto }}
        </p>

        <p v-if="sucessoFoto" class="state-message success">
          Foto atualizada com sucesso!
        </p>

        <div class="divider" />

        <!-- Feedback do formulário -->
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
  width: fit-content;
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

/* Botão de editar foto sobreposto */
.avatar-edit-btn {
  position: absolute;
  bottom: 0;
  right: 0;
  width: 30px;
  height: 30px;
  border-radius: 50%;
  background: #1f8a70;
  color: #fff;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0;
  border: 2px solid #fff;
  cursor: pointer;
  transition: background 0.15s;
}

.avatar-edit-btn:hover {
  background: #19745f;
}

.avatar-edit-btn:disabled {
  background: #94a3b8;
  cursor: not-allowed;
}

/* Spinner de loading */
.spinner {
  width: 12px;
  height: 12px;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-top-color: #fff;
  border-radius: 50%;
  animation: spin 0.7s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

/* Input de arquivo oculto */
.input-foto-oculto {
  display: none;
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

.avatar-hint {
  font-size: 12px;
  color: #64748b;
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