import { createRouter, createWebHistory } from 'vue-router';
import DashboardPage from '../pages/DashboardPage.vue'; // Ajuste o caminho conforme necessário
import ProdutosPage from '../pages/ProdutosPage.vue'; // Ajuste o caminho conforme necessário

export const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      component: DashboardPage
    },
    {
      path: '/produtos', // Rota para a página de produtos
      component: ProdutosPage
    }
  ]
});