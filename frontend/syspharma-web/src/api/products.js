import { api } from './http';

export function mapProductFromApi(product) {
  return {
    id: product.idproduct,
    nome: product.description ?? '',
    codigoBarras: product.internalcode ?? '',
    precoVenda: Number(product.price ?? 0),
    custo: Number(product.cost ?? 0),
    estoque: product.inventories?.[0]?.quantity ?? 0,
    createdAt: product.createdat,
    raw: product
  };
}

export function mapProductToApi(product) {
  return {
    idproduct: product.id ?? 0,
    description: product.nome,
    internalcode: product.codigoBarras,
    price: Number(product.precoVenda ?? 0),
    cost: Number(product.custo ?? 0),
    createdat: product.createdAt ?? new Date().toISOString()
  };
}

export async function getProducts() {
  const { data } = await api.get('/Product');
  return data.map(mapProductFromApi);
}

export async function createProduct(product) {
  const { data } = await api.post('/Product', mapProductToApi(product));
  return mapProductFromApi(data);
}

export async function updateProduct(product) {
  await api.put(`/Product/${product.id}`, mapProductToApi(product));
}

export async function deleteProduct(productId) {
  await api.delete(`/Product/${productId}`);
}
