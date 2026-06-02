import React, { createContext, useContext, useState, useEffect } from 'react';
import vehicleService from '../../services/vehicleService';
import appointmentService from '../../services/appointmentService';
import inventoryService from '../../services/inventoryService';
import maintenanceService from '../../services/maintenanceService';

// ─── Frontend model interfaces ────────────────────────────────────────────────

export interface Vehicle {
  id: string;
  plate: string;
  brand: string;
  model: string;
  year: number;
  color: string;
  ownerName: string;
  ownerPhone: string;
  ownerDocumentNumber: string;
  ownerEmail: string;
  ownerId: string;
  isActive: boolean;
  createdAt: string;
}

export interface VehicleObservation {
  id: string;
  vehicleId: string;
  observation: string;
  mechanicName: string;
  mechanicId?: string;
  date: string;
}

export interface InventoryItem {
  id: string;
  code: string;
  name: string;
  quantity: number;
  minStock: number;
}

export interface Appointment {
  id: string;
  vehicleId: string;
  clientName: string;
  date: string;   // "YYYY-MM-DD"
  time: string;   // "HH:mm"
  description: string;
  mechanicId?: string;
  status: 'pending' | 'in-progress' | 'completed' | 'cancelled';
  observations: string[];
}

// ─── Context type ─────────────────────────────────────────────────────────────

interface DataContextType {
  vehicles: Vehicle[];
  addVehicle: (vehicle: Omit<Vehicle, 'id' | 'createdAt' | 'ownerId'>) => Promise<void>;
  updateVehicle: (id: string, updates: Partial<Vehicle>) => Promise<void>;

  observations: VehicleObservation[];
  loadObservationsForVehicle: (vehicleId: string) => Promise<void>;
  addObservation: (obs: Omit<VehicleObservation, 'id'>) => Promise<void>;

  inventory: InventoryItem[];
  addInventoryItem: (item: Omit<InventoryItem, 'id'>) => Promise<void>;
  updateInventoryItem: (id: string, quantity: number) => Promise<void>;

  appointments: Appointment[];
  addAppointment: (appt: Omit<Appointment, 'id'>) => Promise<void>;
  updateAppointment: (id: string, updates: Partial<Appointment>) => Promise<void>;
  changeAppointmentStatus: (id: string, action: 'start' | 'complete' | 'cancel') => Promise<void>;
  addObservationToAppointment: (appointmentId: string, observation: string, mechanicId: string) => Promise<void>;

  loading: boolean;
  refreshAll: () => Promise<void>;
}

const DataContext = createContext<DataContextType | undefined>(undefined);

// ─── Mapping helpers ──────────────────────────────────────────────────────────

const mapStatus = (s: string): Appointment['status'] => {
  switch (s) {
    case 'Pendiente':  return 'pending';
    case 'EnProceso':  return 'in-progress';
    case 'Completada': return 'completed';
    case 'Cancelada':  return 'cancelled';
    default:           return 'pending';
  }
};

const toTimeString = (time: string): string =>
  time.length === 5 ? `${time}:00` : time; // "10:00" → "10:00:00"

const fromTimeString = (time: string): string =>
  time ? time.substring(0, 5) : ''; // "10:00:00" → "10:00"

const fromDateString = (date: string): string =>
  date ? date.substring(0, 10) : ''; // "2026-03-05T00:00:00Z" → "2026-03-05"

// ─── Provider ─────────────────────────────────────────────────────────────────

export function DataProvider({ children }: { children: React.ReactNode }) {
  const [vehicles, setVehicles]       = useState<Vehicle[]>([]);
  const [observations, setObservations] = useState<VehicleObservation[]>([]);
  const [inventory, setInventory]     = useState<InventoryItem[]>([]);
  const [appointments, setAppointments] = useState<Appointment[]>([]);
  const [loading, setLoading]         = useState(false);

  useEffect(() => {
    const token = localStorage.getItem('autonova_token');
    if (token) refreshAll();
  }, []);

  // ── Loaders ────────────────────────────────────────────────────────────────

  const loadVehicles = async () => {
    try {
      const data = await vehicleService.getAll();
      setVehicles(data.map((v) => ({
        id: v.id,
        plate: v.plate,
        brand: v.brand,
        model: v.model,
        year: v.year,
        color: v.color,
        ownerName: v.owner?.fullName ?? '',
        ownerPhone: v.owner?.phone ?? '',
        ownerDocumentNumber: v.owner?.documentNumber ?? '',
        ownerEmail: v.owner?.email ?? '',
        ownerId: v.owner?.id ?? '',
        isActive: v.isActive,
        createdAt: fromDateString(v.createdAt),
      })));
    } catch {
      setVehicles([]);
    }
  };

  const loadInventory = async () => {
    try {
      const data = await inventoryService.getAll();
      setInventory(data.map((s) => ({
        id: s.id,
        code: s.code,
        name: s.name,
        quantity: s.quantity,
        minStock: s.minimumStock,
      })));
    } catch {
      setInventory([]);
    }
  };

  const loadAppointments = async () => {
    try {
      const data = await appointmentService.getAll();
      setAppointments(data.map((a) => ({
        id: a.id,
        vehicleId: a.vehicleId,
        clientName: a.ownerName ?? '',
        date: fromDateString(a.date),
        time: fromTimeString(a.time),
        description: a.description,
        mechanicId: a.mechanicId,
        status: mapStatus(a.status),
        observations: [],
      })));
    } catch {
      setAppointments([]);
    }
  };

  const refreshAll = async () => {
    setLoading(true);
    await Promise.all([loadVehicles(), loadInventory(), loadAppointments()]);
    setLoading(false);
  };

  // ── Vehicle operations ─────────────────────────────────────────────────────

  const addVehicle = async (vehicle: Omit<Vehicle, 'id' | 'createdAt' | 'ownerId'>) => {
    await vehicleService.create({
      plate: vehicle.plate,
      brand: vehicle.brand,
      model: vehicle.model,
      year: vehicle.year,
      color: vehicle.color || '',
      ownerFullName: vehicle.ownerName,
      ownerDocumentNumber: vehicle.ownerDocumentNumber,
      ownerEmail: vehicle.ownerEmail || '',
      ownerPhone: vehicle.ownerPhone || '',
      ownerAddress: '',
    });
    await loadVehicles();
  };

  const updateVehicle = async (id: string, updates: Partial<Vehicle>) => {
    const v = vehicles.find((x) => x.id === id);
    if (!v) return;

    if (updates.isActive === false) {
      await vehicleService.deactivate(id);
    } else if (updates.isActive === true) {
      await vehicleService.activate(id);
    }

    if (updates.ownerName !== undefined || updates.ownerPhone !== undefined) {
      await vehicleService.updateOwner(v.ownerId, {
        fullName: updates.ownerName ?? v.ownerName,
        documentNumber: v.ownerDocumentNumber,
        email: v.ownerEmail || '',
        phone: updates.ownerPhone ?? v.ownerPhone,
        address: '',
      });
    }

    if (updates.plate || updates.brand || updates.model || updates.year || updates.color) {
      await vehicleService.update(id, {
        plate: updates.plate ?? v.plate,
        brand: updates.brand ?? v.brand,
        model: updates.model ?? v.model,
        year: updates.year ?? v.year,
        color: updates.color ?? v.color,
      });
    }

    await loadVehicles();
  };

  // ── Observation / Maintenance operations ───────────────────────────────────

  const loadObservationsForVehicle = async (vehicleId: string) => {
    try {
      const data = await maintenanceService.getByVehicleId(vehicleId);
      const mapped: VehicleObservation[] = data.map((m) => ({
        id: m.id,
        vehicleId: m.vehicleId,
        observation: m.observation || m.servicePerformed,
        mechanicName: m.mechanicName,
        mechanicId: m.mechanicId,
        date: fromDateString(m.createdAt),
      }));
      setObservations((prev) => {
        const filtered = prev.filter((o) => o.vehicleId !== vehicleId);
        return [...filtered, ...mapped];
      });
    } catch {
      /* silent */
    }
  };

  const addObservation = async (obs: Omit<VehicleObservation, 'id'>) => {
    await maintenanceService.create(
      obs.vehicleId,
      obs.observation,
      obs.observation,
      obs.mechanicId ?? ''
    );
    await loadObservationsForVehicle(obs.vehicleId);
  };

  // ── Inventory operations ───────────────────────────────────────────────────

  const addInventoryItem = async (item: Omit<InventoryItem, 'id'>) => {
    await inventoryService.create(item.code, item.name, item.quantity, item.minStock);
    await loadInventory();
  };

  const updateInventoryItem = async (id: string, newQuantity: number) => {
    const item = inventory.find((i) => i.id === id);
    if (!item) return;
    const delta = newQuantity - item.quantity;
    if (delta > 0) {
      await inventoryService.addStock(id, delta);
    } else if (delta < 0) {
      await inventoryService.withdrawStock(id, Math.abs(delta));
    }
    await loadInventory();
  };

  // ── Appointment operations ─────────────────────────────────────────────────

  const addAppointment = async (appt: Omit<Appointment, 'id'>) => {
    await appointmentService.create({
      date: appt.date,
      time: toTimeString(appt.time),
      description: appt.description,
      vehicleId: appt.vehicleId,
      mechanicId: appt.mechanicId || null,
    });
    await loadAppointments();
  };

  const updateAppointment = async (id: string, updates: Partial<Appointment>) => {
    const appt = appointments.find((a) => a.id === id);
    if (!appt) return;

    if (updates.status && Object.keys(updates).length === 1) {
      // Status-only update: delegate to changeAppointmentStatus
      const action =
        updates.status === 'in-progress' ? 'start' :
        updates.status === 'completed'   ? 'complete' :
        updates.status === 'cancelled'   ? 'cancel'  : null;
      if (action) await appointmentService.changeStatus(id, action);
    } else {
      await appointmentService.update(id, {
        date: updates.date ?? appt.date,
        time: toTimeString(updates.time ?? appt.time),
        description: updates.description ?? appt.description ?? '',
        vehicleId: updates.vehicleId ?? appt.vehicleId,
        mechanicId: updates.mechanicId !== undefined ? updates.mechanicId || null : appt.mechanicId || null,
      });
    }
    await loadAppointments();
  };

  const changeAppointmentStatus = async (
    id: string,
    action: 'start' | 'complete' | 'cancel'
  ) => {
    await appointmentService.changeStatus(id, action);
    await loadAppointments();
  };

  const addObservationToAppointment = async (
    appointmentId: string,
    observation: string,
    mechanicId: string
  ) => {
    const appt = appointments.find((a) => a.id === appointmentId);
    if (!appt) return;

    await maintenanceService.create(
      appt.vehicleId,
      observation,
      observation,
      mechanicId
    );

    // Update local state optimistically
    setAppointments((prev) =>
      prev.map((a) =>
        a.id === appointmentId
          ? { ...a, observations: [...(a.observations || []), observation] }
          : a
      )
    );
  };

  return (
    <DataContext.Provider
      value={{
        vehicles, addVehicle, updateVehicle,
        observations, loadObservationsForVehicle, addObservation,
        inventory, addInventoryItem, updateInventoryItem,
        appointments, addAppointment, updateAppointment,
        changeAppointmentStatus, addObservationToAppointment,
        loading, refreshAll,
      }}
    >
      {children}
    </DataContext.Provider>
  );
}

export function useData() {
  const context = useContext(DataContext);
  if (!context) throw new Error('useData must be used within a DataProvider');
  return context;
}
