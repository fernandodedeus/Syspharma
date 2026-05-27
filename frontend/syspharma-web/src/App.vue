<script setup>
import AppSidebar from './components/AppSidebar.vue';
</script>
<!-- O App.vue é o layout principal da aplicação, ele é responsável por renderizar o menu lateral e a área de conteúdo onde as páginas serão exibidas -->
<template>
  <div class="app-shell">
    <AppSidebar /> <!-- Componente do menu lateral, que fica fixo em todas as páginas -->

    <main class="content">
      <RouterView /> <!-- Componente do Vue Router que renderiza a página correspondente à rota atual -->
    </main>
  </div>
</template>