import { api } from './http';

// essa função mapeia os dados do usuário recebidos da API para o formato utilizado na aplicação
// detecta automaticamente se é resposta de admin (todos os campos) ou usuário padrão (nome e foto apenas)
export function mapUserFromApi(user) {
  // Contrato admin — retorna todos os campos
  if (user.role !== undefined) {
    return {
      id: user.iduser,
      idLoja: user.idstore ?? null,
      role: user.role ?? null,
      nome: user.name,
      email: user.email ?? '',
      cpf: user.cpf ?? '',
      telefone: user.phone ?? '',
      ativo: user.active,
      foto: user.profilephotopath ?? null,
      criadoEm: user.createdat
    };
  }

  // Contrato usuário padrão — retorna apenas nome e foto
  return {
    id: user.iduser,
    nome: user.name,
    foto: user.profilePhotoPath ?? user.profilephotopath ?? null,
    ativo: user.active
  };
}

// essa função mapeia os dados do usuário da aplicação para o formato esperado pela API
export function mapUserToApi(user) {
  return {
    iduser: user.id ?? 0,
    idstore: user.idLoja ?? null,
    name: user.nome,
    email: user.email,
    cpf: user.cpf ?? null,
    pass: user.senha ?? '',
    phone: user.telefone ?? null,
    active: user.ativo ?? true,
    createdat: user.criadoEm ?? new Date().toISOString()
  };
}

export async function getUsers() {
  const { data } = await api.get('/User');
  return data.map(mapUserFromApi);
}

export async function getUser(id) {
  const { data } = await api.get(`/User/${id}`);
  return mapUserFromApi(data);
}

// usa o endpoint POST /Auth/register em vez de POST /User direto, porque o register já faz o hash da senha automaticamente no backend. Se usássemos o POST /User, a senha iria em texto puro.
export async function createUser(user) {
  const { data } = await api.post('/Auth/register', {
    idstore: user.idLoja ?? null,
    name: user.nome,
    email: user.email,
    password: user.senha,
    phone: user.telefone ?? null,
    document: user.cpf ?? null
  });
  return data;
}

export async function updateUser(user) {
  await api.put(`/User/${user.id}`, mapUserToApi(user));
}

// chama o endpoint POST /Auth/switchpass para trocar a senha do usuário logado. O backend vai verificar se a senha antiga está correta antes de atualizar para a nova senha.
export async function switchPassword(oldpass, newpass) {
  const { data } = await api.post('/Auth/switchpass', { oldpass, newpass });
  return data;
}

export async function deleteUser(id) {
  await api.delete(`/User/${id}`);
}

// cria um FormData com o arquivo selecionado e envia via PATCH para o endpoint do backend. O header multipart/form-data é necessário para envio de arquivos.
export async function uploadPhoto(userId, file) {
  const formData = new FormData();
  formData.append('image', file);

  const { data } = await api.patch(`/User/${userId}/photo`, formData, {
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  });

  return data; // retorna o caminho relativo ex: "imgs/foto.jpg"
}