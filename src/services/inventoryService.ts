import api from './api';

export interface SparePartResponse {
  id: string;
  code: string;
  name: string;
  quantity: number;
  minimumStock: number;
  isLowStock: boolean;
  createdAt: string;
}

export interface StockOperationResponse {
  part: SparePartResponse;
  alerts: string[];
  hasAlerts: boolean;
}

const inventoryService = {
  getAll: async (): Promise<SparePartResponse[]> => {
    const { data } = await api.get<SparePartResponse[]>('/sparepart');
    return data;
  },

  create: async (
    code: string,
    name: string,
    quantity: number,
    minimumStock: number
  ): Promise<StockOperationResponse> => {
    const { data } = await api.post<StockOperationResponse>('/sparepart', {
      code,
      name,
      quantity,
      minimumStock,
    });
    return data;
  },

  addStock: async (id: string, amount: number): Promise<StockOperationResponse> => {
    const { data } = await api.patch<StockOperationResponse>(
      `/sparepart/${id}/stock/add`,
      { amount }
    );
    return data;
  },

  withdrawStock: async (id: string, amount: number): Promise<StockOperationResponse> => {
    const { data } = await api.patch<StockOperationResponse>(
      `/sparepart/${id}/stock/withdraw`,
      { amount }
    );
    return data;
  },
};

export default inventoryService;
