import api from './api';

export interface UserResponse {
  id: string;
  name: string;
  email: string;
  role: string; // "Administrador" | "Recepcionista" | "Mecanico"
  isActive: boolean;
  createdAt: string;
}

const userService = {
  // Solo admin
  getAll: async (): Promise<UserResponse[]> => {
    const { data } = await api.get<UserResponse[]>('/user');
    return data;
  },

  // Todos los roles autenticados (para dropdown de mecánicos en citas)
  getMechanics: async (): Promise<UserResponse[]> => {
    const { data } = await api.get<UserResponse[]>('/user/mechanics');
    return data;
  },

  create: async (name: string, email: string, password: string, role: string): Promise<UserResponse> => {
    const { data } = await api.post<UserResponse>('/user', { name, email, password, role });
    return data;
  },

  deactivate: async (id: string): Promise<void> => {
    await api.delete(`/user/${id}`);
  },

  activate: async (id: string): Promise<void> => {
    await api.patch(`/user/${id}/activate`);
  },
};

export default userService;
