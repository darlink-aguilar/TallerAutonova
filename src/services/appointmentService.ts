import api from './api';

export interface AppointmentResponse {
  id: string;
  date: string;
  time: string;
  description: string;
  status: string; // "Pendiente" | "EnProceso" | "Completada" | "Cancelada"
  createdAt: string;
  vehicleId: string;
  plate: string;
  vehicleBrand: string;
  vehicleModel: string;
  ownerName: string;
  mechanicId?: string;
  mechanicName?: string;
}

export interface CreateAppointmentRequest {
  date: string;
  time: string; // "HH:mm:ss"
  description: string;
  vehicleId: string;
  mechanicId?: string | null;
}

export interface UpdateAppointmentRequest {
  date: string;
  time: string; // "HH:mm:ss"
  description: string;
  vehicleId: string;
  mechanicId?: string | null;
}

const appointmentService = {
  getAll: async (): Promise<AppointmentResponse[]> => {
    const { data } = await api.get<AppointmentResponse[]>('/appointment');
    return data;
  },

  create: async (req: CreateAppointmentRequest): Promise<AppointmentResponse> => {
    const { data } = await api.post<AppointmentResponse>('/appointment', req);
    return data;
  },

  update: async (id: string, req: UpdateAppointmentRequest): Promise<AppointmentResponse> => {
    const { data } = await api.put<AppointmentResponse>(`/appointment/${id}`, req);
    return data;
  },

  changeStatus: async (id: string, action: 'start' | 'complete' | 'cancel'): Promise<void> => {
    await api.patch(`/appointment/${id}/status`, { action });
  },

  cancel: async (id: string): Promise<void> => {
    await api.delete(`/appointment/${id}`);
  },
};

export default appointmentService;
