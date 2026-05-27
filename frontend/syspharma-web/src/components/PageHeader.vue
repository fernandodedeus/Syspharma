<script setup>
    defineProps({
                    title: { // propriedade que define o título do cabeçalho, é obrigatória
                        type: String,
                        required: true
                    },
                    subtitle: { // propriedade que define a descrição abaixo do titulo
                        type: String,
                        default: ''
        }
    });
</script>

<template>
  <header class="page-header">
    <div>
      <h1>{{ title }}</h1> 
      <p v-if="subtitle">{{ subtitle }}</p>
    </div>

    <div class="page-header-actions">
      <slot /> <!-- espaço opcional para botões ou ações -->
    </div>
  </header>
</template>