<script setup>
import { ref } from 'vue';
import NfcePanel from '../components/NfcePanel.vue';

const painelAberto = ref(false);
</script>

<template>
  <div class="notas-fiscais-page">
    <div class="page-header">
      <div>
        <h1>Notas Fiscais (NF-e / NFC-e)</h1>
        <p class="subtitle">Gerenciamento e emissão de Notas Fiscais Eletrônicas</p>
      </div>
      <button class="btn-primary" @click="painelAberto = true">
        + Nova NFC-e
      </button>
    </div>

    <div class="content-card">
      <div class="empty-state">
        <svg xmlns="http://www.w3.org/2000/svg" width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">
          <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/>
          <polyline points="14 2 14 8 20 8"/>
          <line x1="16" y1="13" x2="8" y2="13"/>
          <line x1="16" y1="17" x2="8" y2="17"/>
          <polyline points="10 9 9 9 8 9"/>
        </svg>
        <h3>Nenhuma nota fiscal emitida recentemente</h3>
        <p>Clique no botão acima para abrir a emissão de NFC-e.</p>
      </div>
    </div>

    <NfcePanel :aberto="painelAberto" @fechar="painelAberto = false" />
  </div>
</template>

<style scoped>
.notas-fiscais-page {
  display: flex;
  flex-direction: column;
  gap: 24px;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.page-header h1 {
  font-size: 24px;
  font-weight: 700;
  color: #172033;
  margin: 0 0 4px;
}

.subtitle {
  color: #64748b;
  margin: 0;
  font-size: 14px;
}

.content-card {
  background: #ffffff;
  border-radius: 12px;
  padding: 48px 24px;
  border: 1px solid #e1e7ef;
  display: flex;
  justify-content: center;
  align-items: center;
}

.empty-state {
  text-align: center;
  color: #64748b;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 8px;
}

.empty-state svg {
  color: #94a3b8;
}

.empty-state h3 {
  font-size: 16px;
  font-weight: 600;
  color: #334155;
  margin: 8px 0 0;
}

.empty-state p {
  font-size: 14px;
  margin: 0;
}
</style>
