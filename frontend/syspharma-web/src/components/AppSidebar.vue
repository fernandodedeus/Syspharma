<script setup>
import { ref } from 'vue';
import UserMenu from './UserMenu.vue';

// controla se o submenu "Cadastro" está aberto ou fechado, o chevon vai rotacionar de acordo com o estado
const cadastroAberto = ref(false);
</script>

<template>
  <aside class="sidebar">
    <div class="brand">
      <img
        class="brand-logo"
        src="/syspharma-logo-verde-nova.png"
        alt="Syspharma"
      />
    </div>

    <nav class="sidebar-nav">
      <RouterLink to="/">Dashboard</RouterLink>
      <RouterLink to="/produtos">Produtos</RouterLink>
      <RouterLink to="/validades">Validades</RouterLink>

      <!-- Submenu Cadastro -->
      <div class="nav-group">
        <button
          class="nav-group-trigger"
          type="button"
          @click="cadastroAberto = !cadastroAberto"
        >
          <span>Cadastro</span>
          <svg
            :class="['chevron', { rotacionado: cadastroAberto }]"
            xmlns="http://www.w3.org/2000/svg"
            width="14"
            height="14"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            stroke-width="2"
            stroke-linecap="round"
            stroke-linejoin="round"
          >
            <polyline points="6 9 12 15 18 9" />
          </svg>
        </button>

        <div v-if="cadastroAberto" class="nav-group-items">
          <RouterLink to="/cadastro/funcionarios">
            Funcionários
          </RouterLink>
        </div>
      </div>
    </nav>

    <!-- Card de perfil substituindo o botão de sair -->
    <UserMenu />
  </aside>
</template>

<style scoped>
/* Submenu */
.nav-group {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.nav-group-trigger {
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
  background: transparent;
  color: #dbe5f0;
  font-size: 14px;
  font-weight: 500;
  padding: 12px 14px;
  border-radius: 8px;
  cursor: pointer;
  text-align: left;
  transition: background 0.15s;
}

.nav-group-trigger:hover {
  background: rgba(255, 255, 255, 0.06);
}

.nav-group-items {
  display: flex;
  flex-direction: column;
  gap: 4px;
  padding-left: 14px;
}

.nav-group-items a {
  display: block;
  padding: 10px 14px;
  border-radius: 8px;
  color: #aeb9c7;
  font-size: 13px;
  position: relative;
}

.nav-group-items a::before {
  content: '';
  position: absolute;
  left: 4px;
  top: 50%;
  transform: translateY(-50%);
  width: 4px;
  height: 4px;
  border-radius: 50%;
  background: #aeb9c7;
}

.nav-group-items a.router-link-active {
  background: #1f8a70;
  color: #fff;
}

.nav-group-items a.router-link-active::before {
  background: #fff;
}

/* Chevron do submenu */
.chevron {
  color: #aeb9c7;
  flex-shrink: 0;
  transition: transform 0.2s;
}

.chevron.rotacionado {
  transform: rotate(180deg);
}
</style>