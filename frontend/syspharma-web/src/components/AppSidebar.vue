<script setup>
import { useRouter } from 'vue-router';
import { useAuthStore } from '../stores/authStore';

const router = useRouter();
const auth = useAuthStore();

async function handleLogout() {
  await auth.logout();
  router.push('/login');
}
</script>

<template>
  <aside class="sidebar">
    <div class="brand">
      <img
        class="brand-logo"
        src="/syspharma-logo-verde-nova.png"
        alt="Syspharma"
      />
      <span>SaaS Farmácias</span>
    </div>

    <nav class="sidebar-nav">
      <RouterLink to="/">Dashboard</RouterLink>
      <RouterLink to="/produtos">Produtos</RouterLink>
    </nav>

    <div class="sidebar-footer">
      <span v-if="auth.user">{{ auth.user.name }}</span>
      <button class="logout-button" type="button" @click="handleLogout">
        Sair
      </button>
    </div>
  </aside>
</template>
