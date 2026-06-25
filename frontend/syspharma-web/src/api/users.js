import { api } from './http';

// essa função mapeia os dados do usuário recebidos da API para o formato utilizado na aplicação
export function mapUserFromApi(user) {
  return {
    id: user.iduser,
    idLoja: user.idstore ?? null,
    nome: user.name,
    email: user.email,
    cpf: user.cpf ?? '',
    telefone: user.phone ?? '',
    ativo: user.active,
    criadoEm: user.createdat
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