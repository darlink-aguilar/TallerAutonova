import api from './api';

export interface OwnerResponse {
  id: string;
  fullName: string;
  documentNumber: string;
  email: string;
  phone: string;
  address: string;
  vehicleId: string;
}

export interface VehicleResponse {
  id: string;
  plate: string;
  brand: string;
  model: string;
  year: number;
  color: string;
  isActive: boolean;
  createdAt: string;
  owner: OwnerResponse;
}

export interface CreateVehicleRequest {
  plate: string;
  brand: string;
  model: string;
  year: number;
  color: string;
  ownerFullName: string;
  ownerDocumentNumber: string;
  ownerEmail: string;
  ownerPhone: string;
  ownerAddress: string;
}

export interface UpdateVehicleRequest {
  plate: string;
  brand: string;
  model: string;
  year: number;
  color: string;
}

export interface UpdateOwnerRequest {
  fullName: string;
  documentNumber: string;
  email: string;
  phone: string;
  address: string;
}

const vehicleService = {
  getAll: async (): Promise<VehicleResponse[]> => {
    const { data } = await api.get<VehicleResponse[]>('/vehicle');
    return data;
  },

  getById: async (id: string): Promise<VehicleResponse> => {
    const { data } = await api.get<VehicleResponse>(`/vehicle/${id}`);
    return data;
  },

  create: async (req: CreateVehicleRequest): Promise<VehicleResponse> => {
    const { data } = await api.post<VehicleResponse>('/vehicle', req);
    return data;
  },

  update: async (id: string, req: UpdateVehicleRequest): Promise<VehicleResponse> => {
    const { data } = await api.put<VehicleResponse>(`/vehicle/${id}`, req);
    return data;
  },

  deactivate: async (id: string): Promise<void> => {
    await api.delete(`/vehicle/${id}`);
  },

  activate: async (id: string): Promise<void> => {
    await api.patch(`/vehicle/${id}/activate`);
  },

  updateOwner: async (ownerId: string, req: UpdateOwnerRequest): Promise<void> => {
    await api.put(`/owner/${ownerId}`, req);
  },
};

export default vehicleService;
