<script setup>
defineProps({
  tone: {
    type: String,
    default: 'ok'
  }
});
</script>

<template>
  <span :class="['badge', tone]">
    <slot /> <!-- o slot permite que o conteúdo seja passado para o componente -->
  </span>
</template>

<style scoped>
.badge {
  display: inline-block;
  border-radius: 999px;
  padding: 5px 10px;
  font-size: 13px;
  font-weight: 700;
  white-space: nowrap;
}

.badge.ok {
  background: #dcfce7;
  color: #166534;
}

.badge.low {
  background: #fee2e2;
  color: #991b1b;
}

.badge.warning {
  background: #fef3c7;
  color: #92400e;
}
</style>