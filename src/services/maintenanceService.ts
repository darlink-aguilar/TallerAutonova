import api from './api';

export interface MaintenanceHistoryResponse {
  id: string;
  vehicleId: string;
  plate: string;
  brand: string;
  model: string;
  ownerName: string;
  observation: string;
  servicePerformed: string;
  createdAt: string;
  mechanicId: string;
  mechanicName: string;
}

const maintenanceService = {
  getByVehicleId: async (vehicleId: string): Promise<MaintenanceHistoryResponse[]> => {
    const { data } = await api.get<MaintenanceHistoryResponse[]>(
      `/maintenancehistory/vehicle/${vehicleId}`
    );
    return data;
  },

  getByPlate: async (plate: string): Promise<MaintenanceHistoryResponse[]> => {
    const { data } = await api.get<MaintenanceHistoryResponse[]>(
      `/maintenancehistory/plate/${plate}`
    );
    return data;
  },

  create: async (
    vehicleId: string,
    observation: string,
    servicePerformed: string,
    mechanicId: string
  ): Promise<MaintenanceHistoryResponse> => {
    const { data } = await api.post<MaintenanceHistoryResponse>('/maintenancehistory', {
      vehicleId,
      observation,
      servicePerformed,
      mechanicId,
    });
    return data;
  },
};

export default maintenanceService;
