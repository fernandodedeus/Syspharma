import { api } from './http';

// busca a lista de fornecedores para popular o dropdown de fornecedores nos lotes, seguindo o mesmo padrao do products.js e batches.js

export function mapSupplierFromApi(supplier) {
  return {
    id: supplier.idsupplier,
    nome: supplier.name,
    nomeFantasia: supplier.tradename ?? '',
    documento: supplier.document ?? '',
    email: supplier.email ?? '',
    telefone: supplier.phone ?? '',
    ativo: supplier.active ?? true
  };
}

export async function getSuppliers() {
  const { data } = await api.get('/Supplier');
  return data.map(mapSupplierFromApi);
}