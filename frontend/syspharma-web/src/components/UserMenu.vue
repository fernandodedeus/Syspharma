<script setup>
import { ref, computed, onMounted, onUnmounted } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '../stores/authStore';

const router = useRouter();
const auth = useAuthStore();

const menuAberto = ref(false);

// Inicial do nome para fallback caso não tenha foto
const inicialNome = computed(() => {
  return auth.user?.name?.charAt(0).toUpperCase() ?? '?';
});

// Fecha o menu ao clicar fora dele
function handleClickFora(event) {
  const menu = document.getElementById('user-menu');
  if (menu && !menu.contains(event.target)) {
    menuAberto.value = false;
  }
}

async function handleLogout() {
  menuAberto.value = false;
  await auth.logout();
  router.push('/login');
}

function navegarPara(rota) {
  menuAberto.value = false;
  router.push(rota);
}

onMounted(() => {
  document.addEventListener('click', handleClickFora);
});

onUnmounted(() => {
  document.removeEventListener('click', handleClickFora);
});
</script>

<template>
  <div id="user-menu" class="user-menu">

    <!-- Botão que abre o menu -->
    <button
      class="user-menu-trigger"
      type="button"
      @click="menuAberto = !menuAberto"
    >
      <div class="avatar">
        <img
          v-if="auth.photoUrl"
          :src="auth.photoUrl"
          :alt="auth.user?.name"
          class="avatar-img"
        />
        <span v-else>{{ inicialNome }}</span>
      </div>

      <div class="user-info">
        <span class="user-name">{{ auth.user?.name }}</span>
        <span class="user-role">Funcionário</span>
      </div>

      <svg
        :class="['chevron', { rotacionado: menuAberto }]"
        xmlns="http://www.w3.org/2000/svg"
        width="16"
        height="16"
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

    <!-- Dropdown do menu -->
    <div v-if="menuAberto" class="user-menu-dropdown">
      <div class="dropdown-header">
        <div class="avatar avatar-lg">
          <img
            v-if="auth.photoUrl"
            :src="auth.photoUrl"
            :alt="auth.user?.name"
            class="avatar-img"
          />
          <span v-else>{{ inicialNome }}</span>
        </div>

        <div>
          <span class="dropdown-name">{{ auth.user?.name }}</span>
          <span class="dropdown-email">{{ auth.user?.email }}</span>
        </div>
      </div>

      <div class="dropdown-divider" />

      <button
        class="dropdown-item"
        type="button"
        @click="navegarPara('/perfil')"
      >
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/>
          <circle cx="12" cy="7" r="4"/>
        </svg>
        Editar Perfil
      </button>

      <button
        class="dropdown-item"
        type="button"
        @click="navegarPara('/trocar-senha')"
      >
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <rect x="3" y="11" width="18" height="11" rx="2" ry="2"/>
          <path d="M7 11V7a5 5 0 0 1 10 0v4"/>
        </svg>
        Trocar Senha
      </button>

      <div class="dropdown-divider" />

      <button
        class="dropdown-item danger"
        type="button"
        @click="handleLogout"
      >
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/>
          <polyline points="16 17 21 12 16 7"/>
          <line x1="21" y1="12" x2="9" y2="12"/>
        </svg>
        Sair
      </button>
    </div>

  </div>
</template>

<style scoped>
.user-menu {
  position: relative;
  margin-top: auto;
}

.user-menu-trigger {
  width: 100%;
  display: flex;
  align-items: center;
  gap: 10px;
  background: #1a3348;
  border-radius: 10px;
  padding: 10px 12px;
  cursor: pointer;
  text-align: left;
  transition: background 0.15s;
}

.user-menu-trigger:hover {
  background: #22405a;
}

.avatar {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: #1f8a70;
  color: #fff;
  font-size: 14px;
  font-weight: 700;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  overflow: hidden;
}

.avatar-lg {
  width: 48px;
  height: 48px;
  font-size: 18px;
}

.avatar-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.user-info {
  flex: 1;
  min-width: 0;
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.user-name {
  font-size: 13px;
  font-weight: 700;
  color: #fff;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.user-role {
  font-size: 11px;
  color: #aeb9c7;
}

.chevron {
  color: #aeb9c7;
  flex-shrink: 0;
  transition: transform 0.2s;
}

.chevron.rotacionado {
  transform: rotate(180deg);
}

.user-menu-dropdown {
  position: absolute;
  bottom: calc(100% + 8px);
  left: 0;
  right: 0;
  background: #fff;
  border: 1px solid #e1e7ef;
  border-radius: 10px;
  padding: 8px;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.12);
  z-index: 100;
}

.dropdown-header {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 8px 6px 12px;
}

.dropdown-name {
  display: block;
  font-size: 14px;
  font-weight: 700;
  color: #172033;
}

.dropdown-email {
  display: block;
  font-size: 12px;
  color: #64748b;
  margin-top: 2px;
}

.dropdown-divider {
  height: 1px;
  background: #e1e7ef;
  margin: 4px 0;
}

.dropdown-item {
  width: 100%;
  display: flex;
  align-items: center;
  gap: 10px;
  background: transparent;
  color: #172033;
  font-size: 14px;
  font-weight: 500;
  padding: 10px;
  border-radius: 8px;
  text-align: left;
  cursor: pointer;
  transition: background 0.15s;
}

.dropdown-item:hover {
  background: #f1f5f9;
}

.dropdown-item.danger {
  color: #dc2626;
}

.dropdown-item.danger:hover {
  background: #fef2f2;
}
</style>