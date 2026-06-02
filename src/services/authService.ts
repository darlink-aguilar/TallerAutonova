import api from './api';

export interface LoginResponse {
  token: string;
  role: string;
  name: string;
  email: string;
  userId: string;
  expiration: string;
}

const authService = {
  login: async (email: string, password: string): Promise<LoginResponse> => {
    const { data } = await api.post<LoginResponse>('/auth/login', { email, password });
    return data;
  },
};

export default authService;
