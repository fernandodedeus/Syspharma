<script setup>
            defineProps({
            label: { // texto pequeno do card
                type: String,
                required: true
            },
            value: { // número principal
                type: [String, Number],
                required: true
            },
            tone: { // variação visual: default, warning, danger, success
                type: String,
                default: 'default'
            }
            });
</script>

<template>
  <article :class="['metric-card', tone]">
    <span>{{ label }}</span>
    <strong>{{ value }}</strong>
  </article>
</template>

<style scoped>
.metric-card span {
  display: block;
  color: #475569;
  font-size: 14px;
  font-weight: 700;
  line-height: 1.35;
}

.metric-card strong {
  font-size: clamp(26px, 3vw, 34px);
  line-height: 1.1;
  overflow-wrap: anywhere;
}
</style>