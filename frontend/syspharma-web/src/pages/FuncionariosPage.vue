<script setup>
import { onMounted, ref, computed } from 'vue';
import { getUsers, createUser, updateUser, deleteUser } from '../api/users';
import { getStores } from '../api/stores';
import { useAuthStore } from '../stores/authStore';
import PageHeader from '../components/PageHeader.vue';
import UserForm from '../components/UserForm.vue';
import UsersTable from '../components/UsersTable.vue';

const auth = useAuthStore();

const usuarios = ref([]);
const lojas = ref([]);
const busca = ref('');
const carregando = ref(false);
const salvando = ref(false);
const erro = ref('');
const usuarioEmEdicao = ref(null);

const usuariosFiltrados = computed(() => {
  const termo = busca.value.trim().toLowerCase();

  if (!termo) return usuarios.value;

  return usuarios.value.filter((usuario) => {
    return (
      usuario.nome.toLowerCase().includes(termo) ||
      (usuario.email ?? '').toLowerCase().includes(termo) ||
      (usuario.cpf ?? '').toLowerCase().includes(termo)
    );
  });
});

async function carregarUsuarios() {
  carregando.value = true;
  erro.value = '';

  try {
    usuarios.value = await getUsers();
  } catch {
    erro.value = 'Não foi possível carregar os funcionários.';
  } finally {
    carregando.value = false;
  }
}

async function carregarLojas() {
  try {
    lojas.value = await getStores();
  } catch {
    console.error('Erro ao carregar lojas.');
  }
}

async function salvarUsuario(dados) {
  salvando.value = true;
  erro.value = '';

  try {
    if (usuarioEmEdicao.value) {
      await updateUser({
        ...usuarioEmEdicao.value,
        ...dados
      });

      await carregarUsuarios();
      usuarioEmEdicao.value = null;
      return;
    }

    await createUser(dados);
    await carregarUsuarios();
  } catch (e) {
    erro.value =
      e.response?.data?.detail ??
      e.response?.data?.errors?.Email?.[0] ??
      'Não foi possível salvar o funcionário.';
  } finally {
    salvando.value = false;
  }
}

function editarUsuario(usuario) {
  usuarioEmEdicao.value = usuario;
  window.scrollTo({ top: 0, behavior: 'smooth' });
}

async function excluirUsuario(usuario) {
  const confirmar = window.confirm(
    `Deseja excluir o funcionário ${usuario.nome}? Essa ação não pode ser desfeita.`
  );

  if (!confirmar) return;

  erro.value = '';

  try {
    await deleteUser(usuario.id);
    usuarios.value = usuarios.value.filter((u) => u.id !== usuario.id);
  } catch {
    erro.value = 'Não foi possível excluir o funcionário.';
  }
}

onMounted(() => {
  carregarUsuarios();

  // Lojas só precisam ser carregadas para admins
  if (auth.isAdmin) {
    carregarLojas();
  }
});
</script>

<template>
  <section>
    <PageHeader
      title="Funcionários"
      subtitle="Cadastro e gerenciamento de funcionários"
    />

    <!-- Formulário visível apenas para admins -->
    <template v-if="auth.isAdmin">
      <p v-if="usuarioEmEdicao" class="edit-message">
        Editando <strong>{{ usuarioEmEdicao.nome }}</strong>. Preencha o formulário e clique em Atualizar.
      </p>

      <UserForm
        :lojas="lojas"
        :loading="salvando"
        :initial-user="usuarioEmEdicao"
        @submit="salvarUsuario"
        @cancel="usuarioEmEdicao = null"
      />
    </template>

    <p v-if="erro" class="state-message error">
      {{ erro }}
    </p>

    <!-- Barra de busca -->
    <section class="card toolbar">
      <label class="search-field" for="buscaFuncionario">
        <span>Buscar funcionário</span>
        <input
          id="buscaFuncionario"
          v-model="busca"
          type="search"
          placeholder="Digite nome, email ou CPF"
        />
      </label>
    </section>

    <!-- Estados da tabela -->
    <p v-if="carregando" class="state-message">
      Carregando funcionários...
    </p>

    <p v-else-if="!usuariosFiltrados.length" class="state-message">
      Nenhum funcionário encontrado.
    </p>

    <UsersTable
      v-else
      :usuarios="usuariosFiltrados"
      @edit="editarUsuario"
      @delete="excluirUsuario"
    />
  </section>
</template>

<style scoped>
.toolbar {
  margin-bottom: 18px;
}

.search-field {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.search-field span {
  color: #475569;
  font-size: 13px;
  font-weight: 700;
}

.search-field input {
  max-width: 420px;
}

.edit-message {
  margin: 0 0 18px;
  background: #fff;
  border: 1px solid #e1e7ef;
  border-radius: 8px;
  padding: 18px;
  color: #64748b;
}

.state-message {
  margin: 0 0 18px;
  background: #fff;
  border: 1px solid #e1e7ef;
  border-radius: 8px;
  padding: 18px;
  color: #64748b;
}

.state-message.error {
  border-color: #fecaca;
  background: #fef2f2;
  color: #991b1b;
}
</style>