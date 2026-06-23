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
      <span>Olá, {{ auth.user?.name }}</span>
    </div>

    <nav class="sidebar-nav">
      <RouterLink to="/">Dashboard</RouterLink>
      <RouterLink to="/produtos">Produtos</RouterLink>
      <RouterLink to="/validades">Validades</RouterLink>
    </nav>

    <div class="sidebar-footer">
      <button class="logout-button" type="button" @click="handleLogout">
        Sair
      </button>
    </div>
  </aside>
</template>
