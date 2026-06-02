import { createRouter, createWebHistory } from 'vue-router';
import DashboardPage from '../pages/DashboardPage.vue';
import LoginPage from '../pages/LoginPage.vue';
import ProdutosPage from '../pages/ProdutosPage.vue';
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
