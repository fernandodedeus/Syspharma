import { createApp } from 'vue';
import { createPinia } from 'pinia';
import './style.css'; // Certifique-se de que o caminho para o arquivo CSS esteja correto
import App from './App.vue'; // Certifique-se de que o caminho para o arquivo App.vue esteja correto
import { router } from './router'; // Certifique-se de que o caminho para o arquivo router/index.js esteja correto

createApp(App)
  .use(createPinia())
  .use(router)
  .mount('#app');
