<template>
  <div v-if="show" class="confirm-overlay">

    <div class="confirm-modal">

      <h3>Excluir produto</h3>

      <p>
        Tem certeza que deseja excluir este produto?
      </p>

      <div class="confirm-actions">

        <button
          class="btn-cancel"
          @click="$emit('cancel')"
        >
          Cancelar
        </button>


        <button
          class="btn-delete"
          @click="$emit('confirm')"
        >
          Excluir
        </button>

      </div>

    </div>

  </div>
</template>


<script setup>

defineProps({
  show: {
    type: Boolean,
    default: false
  }
})


defineEmits([
  'confirm',
  'cancel'
])

</script>


<style scoped>

.confirm-overlay {

  position: fixed;

  inset: 0;

  background: rgba(15, 23, 42, 0.45);

  display: flex;

  align-items: center;

  justify-content: center;

  z-index: 999;

}


.confirm-modal {

  width: 380px;

  background: #fff;

  border-radius: 12px;

  padding: 24px;

  border: 1px solid #e1e7ef;

  box-shadow:
    0 20px 45px rgba(15,23,42,.2);

}


.confirm-modal h3 {

  margin: 0 0 12px;

  color: #1e293b;

}


.confirm-modal p {

  color: #64748b;

  margin-bottom: 24px;

}


.confirm-actions {

  display: flex;

  justify-content: flex-end;

  gap: 12px;

}


.btn-cancel,
.btn-delete {

  border: none;

  border-radius: 8px;

  padding: 10px 18px;

  cursor: pointer;

  font-weight: 700;

}


.btn-cancel {

  background: #e2e8f0;

  color: #334155;

}


.btn-delete {

  background: #dc2626;

  color: white;

}

</style>