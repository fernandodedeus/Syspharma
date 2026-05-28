<script setup>
import StatusBadge from './StatusBadge.vue';

const emit = defineEmits(['edit', 'delete']); // o componente serve para avisar a página de produtos: "O usuário quer editar este produto" ou "O usuário quer excluir este produto"

defineProps({
  produtos: {
    type: Array,
    default: () => []
  }
});

function obterQuantidadeEstoque(produto) {
  return produto.estoque?.quantidade ?? produto.estoque ?? 0;
}

function obterTomEstoque(produto) {
  return obterQuantidadeEstoque(produto) <= 10 ? 'low' : 'ok';
}

function formatarPreco(valor) {
  return Number(valor ?? 0).toFixed(2);
}
</script>

<template>
  <section class="card table-card">
    <table>
      <thead>
        <tr>
          <th>Produto</th>
          <th>Código</th>
          <th>Preço</th>
          <th>Estoque</th>
          <th>Ações</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="produto in produtos" :key="produto.id">
          <td>{{ produto.nome }}</td>
          <td>{{ produto.codigoBarras }}</td>
          <td>R$ {{ formatarPreco(produto.precoVenda) }}</td>
          <td>
            <StatusBadge :tone="obterTomEstoque(produto)">
              {{ obterQuantidadeEstoque(produto) }} un.
            </StatusBadge>
          </td>
          <td>
            <div class="table-actions">
              <button class="action-button secondary" type="button" @click="emit('edit', produto)">
                Editar
              </button>

              <button class="action-button danger" type="button" @click="emit('delete', produto)">
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

@media (max-width: 1000px) {
  table {
    min-width: 680px;
  }
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
</style>