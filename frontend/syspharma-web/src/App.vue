<script setup>
import { computed } from 'vue';
import { useRoute } from 'vue-router';
import AppSidebar from './components/AppSidebar.vue';

const route = useRoute();
const isLoginPage = computed(() => route.path === '/login');
</script>

<template>
  <div class="app-shell" :class="{ 'app-shell-login': isLoginPage }">
    <AppSidebar v-if="!isLoginPage" />

    <main class="content">
      <RouterView />
    </main>
  </div>
</template>
