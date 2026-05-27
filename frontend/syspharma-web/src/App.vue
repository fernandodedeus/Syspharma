<template>
  <div class="app-shell">
    <aside class="sidebar">
      <div class="brand">
        <strong>Syspharma</strong>
        <span>SaaS Farmácias</span>
      </div>

      <nav>
        <RouterLink to="/">Dashboard</RouterLink>
        <RouterLink to="/produtos">Produtos</RouterLink>
      </nav>
    </aside>

    <main class="content">
      <RouterView />
    </main>
  </div>
</template>