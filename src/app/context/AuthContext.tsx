import React, { createContext, useContext, useState, useEffect } from 'react';
import authService from '../../services/authService';
import userService, { UserResponse } from '../../services/userService';

export type UserRole = 'admin' | 'mechanic' | 'receptionist';

export interface User {
  id: string;
  name: string;
  email: string;
  role: UserRole;
  isActive: boolean;
}

interface AuthContextType {
  user: User | null;
  isLoading: boolean;
  login: (email: string, password: string) => Promise<boolean>;
  logout: () => void;
  users: User[];
  loadUsers: () => Promise<void>;
  addUser: (name: string, email: string, password: string, role: UserRole) => Promise<void>;
  updateUser: (id: string, updates: Partial<User>) => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

const mapRole = (backendRole: string): UserRole => {
  if (backendRole === 'Administrador') return 'admin';
  if (backendRole === 'Mecanico')      return 'mechanic';
  if (backendRole === 'Recepcionista') return 'receptionist';
  return 'admin';
};

const mapRoleToBackend = (role: UserRole): string => {
  if (role === 'admin')        return 'Administrador';
  if (role === 'mechanic')     return 'Mecanico';
  if (role === 'receptionist') return 'Recepcionista';
  return 'Administrador';
};

const mapUser = (u: UserResponse): User => ({
  id: u.id, name: u.name, email: u.email,
  role: mapRole(u.role), isActive: u.isActive,
});

// Lee localStorage de forma SÍNCRONA para que DashboardLayout tenga
// el usuario correcto desde el primer render (evita race condition).
const readStoredUser = (): User | null => {
  try {
    const saved = localStorage.getItem('autonova_current_user');
    return saved ? JSON.parse(saved) : null;
  } catch {
    return null;
  }
};

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser]           = useState<User | null>(readStoredUser);
  const [users, setUsers]         = useState<User[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const stored = readStoredUser();
    if (stored) loadUsersForRole(stored.role);
  }, []);

  const loadUsersForRole = async (role: UserRole) => {
    try {
      if (role === 'admin') {
        const data = await userService.getAll();
        setUsers(data.map(mapUser));
      } else {
        // Recepcionista y Mecánico solo necesitan ver mecánicos (dropdown de citas)
        const data = await userService.getMechanics();
        setUsers(data.map(mapUser));
      }
    } catch {
      setUsers([]);
    }
  };

  const loadUsers = async () => {
    const stored = readStoredUser();
    await loadUsersForRole(stored?.role ?? 'admin');
  };

  const login = async (email: string, password: string): Promise<boolean> => {
    setIsLoading(true);
    try {
      const result = await authService.login(email, password);
      localStorage.setItem('autonova_token', result.token);

      const loggedUser: User = {
        id: result.userId, name: result.name,
        email: result.email, role: mapRole(result.role), isActive: true,
      };

      setUser(loggedUser);
      localStorage.setItem('autonova_current_user', JSON.stringify(loggedUser));
      await loadUsersForRole(loggedUser.role);
      return true;
    } catch {
      return false;
    } finally {
      setIsLoading(false);
    }
  };

  const logout = () => {
    setUser(null);
    setUsers([]);
    localStorage.removeItem('autonova_token');
    localStorage.removeItem('autonova_current_user');
  };

  const addUser = async (
    name: string, email: string, password: string, role: UserRole
  ): Promise<void> => {
    await userService.create(name, email, password, mapRoleToBackend(role));
    await loadUsers();
  };

  const updateUser = async (id: string, updates: Partial<User>): Promise<void> => {
    if (updates.isActive === false) await userService.deactivate(id);
    else if (updates.isActive === true) await userService.activate(id);
    await loadUsers();
  };

  return (
    <AuthContext.Provider
      value={{ user, isLoading, login, logout, users, loadUsers, addUser, updateUser }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) throw new Error('useAuth must be used within an AuthProvider');
  return context;
}
