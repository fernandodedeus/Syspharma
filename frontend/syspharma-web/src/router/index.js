import { createRouter, createWebHistory } from 'vue-router';
import DashboardPage from '../pages/DashboardPage.vue';
import ProdutosPage from '../pages/ProdutosPage.vue';

export const router = createRouter({
  history: createWebHistory(),
  routes: [
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