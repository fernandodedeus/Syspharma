<script setup>
defineProps({
  title: {
    type: String,
    required: true
  }
});
</script>

<template>
  <article class="card info-card">
    <h2>{{ title }}</h2>

    <div class="info-card-content">
      <slot /> <!-- esse componente tem uma área livre onde eu posso colocar qualquer conteúdo -->
    </div>
  </article>
</template>

<style scoped>
.info-card h2 {
  margin: 0 0 14px;
  font-size: 18px;
  font-weight: 700;
  color: #172033;
}

.info-card-content {
  color: #475569;
}
</style>