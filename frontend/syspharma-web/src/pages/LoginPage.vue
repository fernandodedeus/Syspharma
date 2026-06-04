<script setup>
import { reactive } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '../stores/authStore';

const router = useRouter();
const auth = useAuthStore();

const form = reactive({
  email: '',
  password: ''
});

async function submitLogin() {
  try {
    await auth.login({
      email: form.email.trim(),
      password: form.password
    });

    router.push('/');
  } catch {
    form.password = '';
  }
}
</script>

<template>
  <section class="login-page">
    <form class="card login-card" @submit.prevent="submitLogin">
      <div class="login-heading">
        <strong>Syspharma</strong>
        <span>Acesse sua operação</span>
      </div>

      <label class="login-field" for="email">
        <span>Email</span>
        <input
          id="email"
          v-model="form.email"
          autocomplete="email"
          required
          type="email"
        />
      </label>

      <label class="login-field" for="password">
        <span>Senha</span>
        <input
          id="password"
          v-model="form.password"
          autocomplete="current-password"
          required
          type="password"
        />
      </label>

      <p v-if="auth.error" class="login-error">
        {{ auth.error }}
      </p>

      <button type="submit" :disabled="auth.loading">
        {{ auth.loading ? 'Entrando...' : 'Entrar' }}
      </button>
    </form>
  </section>
</template>

<style scoped>
.login-page {
  min-height: calc(100vh - 64px);
  display: grid;
  place-items: center;
}

.login-card {
  width: min(100%, 420px);
  display: grid;
  gap: 16px;
}

.login-heading {
  display: grid;
  gap: 4px;
}

.login-heading strong {
  font-size: 26px;
}

.login-heading span,
.login-field span {
  color: #64748b;
  font-size: 13px;
  font-weight: 700;
}

.login-field {
  display: grid;
  gap: 6px;
}

.login-error {
  margin: 0;
  border: 1px solid #fecaca;
  border-radius: 8px;
  background: #fef2f2;
  color: #991b1b;
  padding: 12px;
}

button:disabled {
  background: #94a3b8;
  cursor: not-allowed;
}
</style>
