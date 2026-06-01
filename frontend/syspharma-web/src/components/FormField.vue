<script setup>
defineProps({
  label: {
    type: String,
    required: true
  },
  id: {
    type: String,
    required: true
  },
  type: {
    type: String,
    default: 'text'
  },
  modelValue: {
    type: [String, Number],
    default: ''
  },
  error: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:modelValue']);
</script>

<template>
  <label class="form-field" :for="id">
    <span>{{ label }}</span>

    <input
      :id="id"
      :type="type"
      :value="modelValue"
      :class="{ invalid: error }"
      @input="emit('update:modelValue', $event.target.value)"
    />

    <small v-if="error">{{ error }}</small>
  </label>
</template>

<style scoped>
.form-field {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.form-field span {
  color: #475569;
  font-size: 13px;
  font-weight: 700;
}

.form-field input {
  width: 100%;
}

.form-field input.invalid {
  border-color: #dc2626;
}

.form-field small {
  color: #dc2626;
  font-size: 12px;
  font-weight: 600;
}
</style>