<script setup>
import { computed } from 'vue';
import StatusBadge from './StatusBadge.vue';
import { useAuthStore } from '../stores/authStore';
import { buildPhotoUrl } from '../stores/authStore';

const auth = useAuthStore();

defineProps({
  usuarios: {
    type: Array,
    default: () => []
  }
});

const emit = defineEmits(['edit', 'delete']);

function obterTomSituacao(ativo) {
  return ativo ? 'ok' : 'low';
}

function obterTextoSituacao(ativo) {
  return ativo ? 'Ativo' : 'Inativo';
}

function formatarData(dataIso) {
  if (!dataIso) return '—';
  const [data] = dataIso.split('T');
  const [ano, mes, dia] = data.split('-');
  return `${dia}/${mes}/${ano}`;
}
</script>

<template>
  <section class="card table-card">
    <table>
      <thead>
        <tr>
          <th>Funcionário</th>
          <th v-if="auth.isAdmin">Email</th>
          <th v-if="auth.isAdmin">CPF</th>
          <th v-if="auth.isAdmin">Telefone</th>
          <th>Situação</th>
          <th v-if="auth.isAdmin">Cadastrado em</th>
          <th v-if="auth.isAdmin">Ações</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="usuario in usuarios" :key="usuario.id">
          <td>
            <div class="user-cell">
              <div class="avatar">
                <img
                  v-if="buildPhotoUrl(usuario.foto)"
                  :src="buildPhotoUrl(usuario.foto)"
                  :alt="usuario.nome"
                  class="avatar-img"
                />
                <span v-else>{{ usuario.nome?.charAt(0).toUpperCase() }}</span>
              </div>
              <span>{{ usuario.nome }}</span>
            </div>
          </td>

          <td v-if="auth.isAdmin">{{ usuario.email || '—' }}</td>
          <td v-if="auth.isAdmin">{{ usuario.cpf || '—' }}</td>
          <td v-if="auth.isAdmin">{{ usuario.telefone || '—' }}</td>

          <td>
            <StatusBadge :tone="obterTomSituacao(usuario.ativo)">
              {{ obterTextoSituacao(usuario.ativo) }}
            </StatusBadge>
          </td>

          <td v-if="auth.isAdmin">{{ formatarData(usuario.criadoEm) }}</td>

          <td v-if="auth.isAdmin">
            <div class="table-actions">
              <button
                class="action-button secondary"
                type="button"
                @click="emit('edit', usuario)"
              >
                Editar
              </button>

              <button
                class="action-button danger"
                type="button"
                @click="emit('delete', usuario)"
              >
                Excluir
              </button>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </section>
</template>

<style scoped>
.table-card {
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th {
  color: #64748b;
  font-size: 13px;
  font-weight: 700;
  text-align: left;
}

td,
th {
  border-bottom: 1px solid #e5eaf0;
  padding: 14px 10px;
}

tbody tr:hover {
  background: #f8fafc;
}

.user-cell {
  display: flex;
  align-items: center;
  gap: 10px;
}

.avatar {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  background: #1f8a70;
  color: #fff;
  font-size: 13px;
  font-weight: 700;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  overflow: hidden;
}

.avatar-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.table-actions {
  display: flex;
  gap: 8px;
}

.action-button {
  padding: 8px 10px;
  font-size: 13px;
}

.action-button.secondary {
  background: #e2e8f0;
  color: #172033;
}

.action-button.secondary:hover {
  background: #cbd5e1;
}

.action-button.danger {
  background: #dc2626;
}

.action-button.danger:hover {
  background: #b91c1c;
}

@media (max-width: 1000px) {
  table {
    min-width: 700px;
  }
}
</style>