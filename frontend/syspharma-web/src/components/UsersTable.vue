<script setup>
import StatusBadge from './StatusBadge.vue';

defineProps({
  usuarios: {
    type: Array,
    default: () => []
  }
});

const emit = defineEmits(['edit', 'delete']);

// Retorna o tom do badge com base na situação do usuário
function obterTomSituacao(ativo) {
  return ativo ? 'ok' : 'low';
}

// Retorna o texto do badge com base na situação do usuário
function obterTextoSituacao(ativo) {
  return ativo ? 'Ativo' : 'Inativo';
}

// Formata a data de criação para o padrão brasileiro
function formatarData(dataIso) {
  if (!dataIso) return '—';
  const [data] = dataIso.split('T');
  const [ano, mes, dia] = data.split('-');
  return `${dia}/${mes}/${ano}`;
}

// Gera a URL do Gravatar a partir do email
function gravatarUrl(email) {
  if (!email) return null;

  // Gravatar exige o email em MD5 — usamos a Web Crypto API do navegador
  const encoded = new TextEncoder().encode(email.trim().toLowerCase());

  return crypto.subtle.digest('SHA-256', encoded).then((hash) => {
    const hex = Array.from(new Uint8Array(hash))
      .map((b) => b.toString(16).padStart(2, '0'))
      .join('');
    return `https://www.gravatar.com/avatar/${hex}?d=mp&s=32`;
  });
}
</script>

<template>
  <section class="card table-card">
    <table>
      <thead>
        <tr>
          <th>Funcionário</th>
          <th>Email</th>
          <th>CPF</th>
          <th>Telefone</th>
          <th>Situação</th>
          <th>Cadastrado em</th>
          <th>Ações</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="usuario in usuarios" :key="usuario.id">
          <td>
            <div class="user-cell">
              <!-- Avatar via Gravatar com fallback para iniciais -->
              <div class="avatar">
                {{ usuario.nome?.charAt(0).toUpperCase() }}
              </div>
              <span>{{ usuario.nome }}</span>
            </div>
          </td>
          <td>{{ usuario.email }}</td>
          <td>{{ usuario.cpf || '—' }}</td>
          <td>{{ usuario.telefone || '—' }}</td>
          <td>
            <StatusBadge :tone="obterTomSituacao(usuario.ativo)">
              {{ obterTextoSituacao(usuario.ativo) }}
            </StatusBadge>
          </td>
          <td>{{ formatarData(usuario.criadoEm) }}</td>
          <td>
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

/* Avatar com inicial do nome — fallback simples sem depender de imagem */
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