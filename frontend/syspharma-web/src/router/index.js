import { createRouter, createWebHistory } from 'vue-router';
import DashboardPage from '../pages/DashboardPage.vue';
import LoginPage from '../pages/LoginPage.vue';
import ProdutosPage from '../pages/ProdutosPage.vue';
import ValidadesPage from '../pages/ValidadesPage.vue';
import FuncionariosPage from '../pages/FuncionariosPage.vue';
import EditarPerfilPage from '../pages/EditarPerfilPage.vue';
import TrocarSenhaPage from '../pages/TrocarSenhaPage.vue';
import { useAuthStore } from '../stores/authStore';

export const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/login',
      component: LoginPage,
      meta: { public: true }
    },
    {
      path: '/',
      component: DashboardPage
    },
    {
      path: '/produtos',
      component: ProdutosPage
    },
    {
      path: '/validades',
      component: ValidadesPage
    },
    {
      path: '/cadastro/funcionarios',
      component: FuncionariosPage
    },
    {
      path: '/perfil',
      component: EditarPerfilPage
    },
    {
      path: '/trocar-senha',
      component: TrocarSenhaPage
    }
  ]
});

router.beforeEach((to) => {
  const auth = useAuthStore();

  if (!to.meta.public && !auth.isAuthenticated) {
    return '/login';
  }

  if (to.path === '/login' && auth.isAuthenticated) {
    return '/';
  }

  return true;
});