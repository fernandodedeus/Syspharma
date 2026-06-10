import { api } from './http';

// Converte o lote que vem da API para o formato que usamos no front
export function mapBatchFromApi(batch) {
  return {
    id: batch.idbatch,
    idProduto: batch.idproduct,
    idFornecedor: batch.idsupplier ?? null,
    codigoLote: batch.batchcode,
    dataValidade: batch.expirationdate,
    dataFabricacao: batch.manufacturingdate ?? null,
    quantidade: Number(batch.quantity ?? 0),
    custoUnitario: Number(batch.unitcost ?? 0),
    criadoEm: batch.createdat
  };
}

// Converte o lote do formato front para o formato que a API espera
export function mapBatchToApi(batch) {
  return {
    idbatch: batch.id ?? 0,
    idproduct: batch.idProduto,
    idsupplier: batch.idFornecedor ?? null,
    batchcode: batch.codigoLote,
    expirationdate: batch.dataValidade,
    manufacturingdate: batch.dataFabricacao ?? null,
    quantity: Number(batch.quantidade ?? 0),
    unitcost: Number(batch.custoUnitario ?? 0),
    createdat: batch.criadoEm ?? new Date().toISOString()
  };
}

// Busca todos os lotes
export async function getBatches() {
  const { data } = await api.get('/ProductBatch');
  return data.map(mapBatchFromApi);
}

// Busca lotes próximos do vencimento (usa o endpoint especial do backend)
export async function getExpiringBatches() {
  const { data } = await api.get('/ProductBatch/expiringbatches');
  return data;
}

// Cria um novo lote
export async function createBatch(batch) {
  const { data } = await api.post('/ProductBatch', mapBatchToApi(batch));
  return mapBatchFromApi(data);
}