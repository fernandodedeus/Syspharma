import { api } from './http';

export function mapStoreFromApi(store) {
  return {
    id: store.idstore,
    nomeFantasia: store.fantasyname ?? '',
    razaoSocial: store.socialname,
    cnpj: store.cnpj ?? '',
    email: store.email ?? '',
    telefone: store.phone ?? '',
    ativo: store.active ?? true
  };
}

export async function getStores() {
  const { data } = await api.get('/Store');
  return data.map(mapStoreFromApi);
}