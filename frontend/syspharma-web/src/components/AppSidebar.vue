
<!-- Esse componente separa o menu lateral do arquivo principal, a vantagem é que o App.vue fica mais limpo e, se depois eu quiser adicionar
 mais abas como "Validades", "Vendas" ou "Clientes", só mexer no menu-->
<template>
  <aside class="sidebar">
    <div class="brand">
      <strong>Syspharma</strong>
      <span>SaaS Farmácias</span>
    </div>

    <nav class="sidebar-nav">
      <RouterLink to="/">Dashboard</RouterLink>
      <RouterLink to="/produtos">Produtos</RouterLink>
    </nav>
  </aside>
</template>