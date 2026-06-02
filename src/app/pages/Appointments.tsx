import { useState } from 'react';
import { useData } from '../context/DataContext';
import { useAuth } from '../context/AuthContext';
import { Card, CardContent, CardHeader, CardTitle } from '../components/ui/card';
import { Input } from '../components/ui/input';
import { Button } from '../components/ui/button';
import {
  Table, TableBody, TableCell, TableHead, TableHeader, TableRow,
} from '../components/ui/table';
import { Search, Plus, Calendar, Edit, X, MessageSquare } from 'lucide-react';
import { Badge } from '../components/ui/badge';
import {
  Dialog, DialogContent, DialogDescription, DialogFooter,
  DialogHeader, DialogTitle, DialogTrigger,
} from '../components/ui/dialog';
import {
  Select, SelectContent, SelectItem, SelectTrigger, SelectValue,
} from '../components/ui/select';
import { Label } from '../components/ui/label';
import { Textarea } from '../components/ui/textarea';
import { toast } from 'sonner';

export function Appointments() {
  const { appointments, vehicles, addAppointment, updateAppointment, changeAppointmentStatus, addObservationToAppointment } = useData();
  const { user, users } = useAuth();

  const isReceptionist = user?.role === 'receptionist';
  const isMechanic     = user?.role === 'mechanic';

  const [searchTerm, setSearchTerm]             = useState('');
  const [isAddDialogOpen, setIsAddDialogOpen]   = useState(false);
  const [editingAppointment, setEditingAppointment] = useState<string | null>(null);
  const [observationDialog, setObservationDialog]   = useState<string | null>(null);
  const [newObservation, setNewObservation]     = useState('');
  const [isSubmitting, setIsSubmitting]         = useState(false);

  const emptyForm = { vehicleId: '', clientName: '', date: '', time: '', description: '', mechanicId: '' };
  const [formData, setFormData]     = useState({ ...emptyForm });
  const [editFormData, setEditFormData] = useState({ ...emptyForm });

  const mechanics = users.filter((u) => u.role === 'mechanic' && u.isActive);

  const filteredAppointments = isMechanic
    ? appointments.filter(
        (apt) =>
          apt.mechanicId === user?.id &&
          (apt.clientName.toLowerCase().includes(searchTerm.toLowerCase()) ||
            vehicles.find((v) => v.id === apt.vehicleId)?.plate.toLowerCase().includes(searchTerm.toLowerCase()))
      )
    : appointments.filter(
        (apt) =>
          apt.clientName.toLowerCase().includes(searchTerm.toLowerCase()) ||
          vehicles.find((v) => v.id === apt.vehicleId)?.plate.toLowerCase().includes(searchTerm.toLowerCase())
      );

  const handleAddAppointment = async () => {
    if (!formData.vehicleId || !formData.clientName || !formData.date || !formData.time) {
      toast.error('Por favor complete todos los campos requeridos');
      return;
    }
    const today = new Date(); today.setHours(0, 0, 0, 0);
    if (new Date(formData.date) < today) {
      toast.error('La fecha de la cita no puede ser anterior a la fecha actual');
      return;
    }
    setIsSubmitting(true);
    try {
      await addAppointment({
        vehicleId: formData.vehicleId,
        clientName: formData.clientName,
        date: formData.date,
        time: formData.time,
        description: formData.description,
        mechanicId: formData.mechanicId || undefined,
        status: 'pending',
        observations: [],
      });
      toast.success('Cita creada exitosamente');
      setIsAddDialogOpen(false);
      setFormData({ ...emptyForm });
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Error al crear la cita');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleSaveEdit = async () => {
    if (!editFormData.vehicleId || !editFormData.date || !editFormData.time) {
      toast.error('Por favor complete todos los campos requeridos');
      return;
    }
    if (!editingAppointment) return;
    setIsSubmitting(true);
    try {
      await updateAppointment(editingAppointment, {
        vehicleId: editFormData.vehicleId,
        clientName: editFormData.clientName,
        date: editFormData.date,
        time: editFormData.time,
        description: editFormData.description,
        mechanicId: editFormData.mechanicId || undefined,
      });
      toast.success('Cita actualizada exitosamente');
      setIsAddDialogOpen(false);
      setEditingAppointment(null);
      setEditFormData({ ...emptyForm });
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Error al actualizar la cita');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleUpdateStatus = async (id: string, frontendStatus: string) => {
    const appointment = appointments.find((apt) => apt.id === id);
    if (!appointment) return;
    if (isMechanic) {
      if (appointment.status === 'pending' && frontendStatus === 'completed') {
        toast.error('Debe pasar por "En Proceso" antes de completar');
        return;
      }
      if (appointment.status === 'completed') {
        toast.error('No se puede modificar una cita completada');
        return;
      }
    }
    const action = frontendStatus === 'in-progress' ? 'start' : frontendStatus === 'completed' ? 'complete' : 'cancel';
    try {
      await changeAppointmentStatus(id, action as 'start' | 'complete' | 'cancel');
      toast.success('Estado actualizado');
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'No se puede realizar esta transición de estado');
    }
  };

  const handleCancelAppointment = async (id: string) => {
    const apt = appointments.find((a) => a.id === id);
    if (apt?.status === 'completed') {
      toast.error('No se puede cancelar una cita completada');
      return;
    }
    try {
      await changeAppointmentStatus(id, 'cancel');
      toast.success('Cita cancelada correctamente');
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Error al cancelar la cita');
    }
  };

  const handleAddObservation = async (appointmentId: string) => {
    if (!newObservation.trim()) {
      toast.error('La observación no puede estar vacía');
      return;
    }
    setIsSubmitting(true);
    try {
      await addObservationToAppointment(appointmentId, newObservation, user?.id || '');
      toast.success('Observación agregada');
      setNewObservation('');
      setObservationDialog(null);
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Error al agregar observación');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleEditAppointment = (id: string) => {
    const apt = appointments.find((a) => a.id === id);
    if (!apt) return;
    if (apt.status === 'in-progress' || apt.status === 'completed') {
      toast.error('No se puede modificar una cita en proceso o finalizada');
      return;
    }
    setEditFormData({
      vehicleId: apt.vehicleId, clientName: apt.clientName,
      date: apt.date, time: apt.time,
      description: apt.description, mechanicId: apt.mechanicId || '',
    });
    setEditingAppointment(id);
    setIsAddDialogOpen(true);
  };

  const activeForm   = editingAppointment ? editFormData : formData;
  const setActiveForm = editingAppointment
    ? (v: any) => setEditFormData(v)
    : (v: any) => setFormData(v);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h2 className="text-3xl font-bold text-[#0B2545]">Gestión de Citas</h2>
          <p className="text-gray-600 mt-1">Programación de servicios y mantenimiento</p>
        </div>
        {isReceptionist && (
          <Dialog open={isAddDialogOpen} onOpenChange={(open) => {
            setIsAddDialogOpen(open);
            if (!open) { setEditingAppointment(null); setEditFormData({ ...emptyForm }); }
          }}>
            <DialogTrigger asChild>
              <Button className="bg-[#FCA311] hover:bg-[#FCA311]/90 rounded-lg" onClick={() => setEditingAppointment(null)}>
                <Plus className="w-4 h-4 mr-2" />Crear Cita
              </Button>
            </DialogTrigger>
            <DialogContent className="rounded-lg max-w-2xl">
              <DialogHeader>
                <DialogTitle>{editingAppointment ? 'Editar Cita' : 'Crear Nueva Cita'}</DialogTitle>
                <DialogDescription>{editingAppointment ? 'Modifique los detalles de la cita' : 'Programe un servicio'}</DialogDescription>
              </DialogHeader>
              <div className="grid grid-cols-2 gap-4 py-4">
                <div className="space-y-2 col-span-2">
                  <Label>Nombre del Cliente *</Label>
                  <Input value={activeForm.clientName} onChange={(e) => setActiveForm({ ...activeForm, clientName: e.target.value })} placeholder="Nombre completo" className="rounded-lg" />
                </div>
                <div className="space-y-2 col-span-2">
                  <Label>Vehículo *</Label>
                  <Select value={activeForm.vehicleId} onValueChange={(v) => setActiveForm({ ...activeForm, vehicleId: v })}>
                    <SelectTrigger className="rounded-lg"><SelectValue placeholder="Seleccione un vehículo" /></SelectTrigger>
                    <SelectContent>
                      {vehicles.filter((v) => v.isActive).map((v) => (
                        <SelectItem key={v.id} value={v.id}>{v.plate} - {v.brand} {v.model} ({v.ownerName})</SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
                <div className="space-y-2">
                  <Label>Fecha *</Label>
                  <Input type="date" value={activeForm.date} onChange={(e) => setActiveForm({ ...activeForm, date: e.target.value })} className="rounded-lg" />
                </div>
                <div className="space-y-2">
                  <Label>Hora *</Label>
                  <Input type="time" value={activeForm.time} onChange={(e) => setActiveForm({ ...activeForm, time: e.target.value })} className="rounded-lg" />
                </div>
                <div className="space-y-2 col-span-2">
                  <Label>Mecánico Asignado (Opcional)</Label>
                  <Select value={activeForm.mechanicId} onValueChange={(v) => setActiveForm({ ...activeForm, mechanicId: v })}>
                    <SelectTrigger className="rounded-lg"><SelectValue placeholder="Sin asignar" /></SelectTrigger>
                    <SelectContent>
                      {mechanics.map((m) => <SelectItem key={m.id} value={m.id}>{m.name}</SelectItem>)}
                    </SelectContent>
                  </Select>
                </div>
                <div className="space-y-2 col-span-2">
                  <Label>Descripción del Servicio</Label>
                  <Textarea value={activeForm.description} onChange={(e) => setActiveForm({ ...activeForm, description: e.target.value })} placeholder="Detalle del servicio..." rows={3} className="rounded-lg resize-none" />
                </div>
              </div>
              <DialogFooter>
                <Button onClick={editingAppointment ? handleSaveEdit : handleAddAppointment} disabled={isSubmitting} className="bg-[#FCA311] hover:bg-[#FCA311]/90 rounded-lg">
                  {isSubmitting ? 'Guardando...' : editingAppointment ? 'Guardar Cambios' : 'Guardar Cita'}
                </Button>
              </DialogFooter>
            </DialogContent>
          </Dialog>
        )}
      </div>

      <Card className="rounded-lg shadow-md">
        <CardContent className="pt-6">
          <div className="relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 w-5 h-5" />
            <Input placeholder="Buscar por cliente o placa..." value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} className="pl-10 rounded-lg" />
          </div>
        </CardContent>
      </Card>

      <Card className="rounded-lg shadow-md">
        <CardHeader><CardTitle>{isMechanic ? 'Mis Citas Programadas' : 'Lista de Citas'}</CardTitle></CardHeader>
        <CardContent>
          <div className="rounded-lg border">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Fecha</TableHead><TableHead>Hora</TableHead>
                  <TableHead>Cliente</TableHead><TableHead>Vehículo</TableHead>
                  <TableHead>Descripción</TableHead>
                  {!isMechanic && <TableHead>Mecánico</TableHead>}
                  <TableHead className="text-center">Estado</TableHead>
                  <TableHead className="text-center">Acciones</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {filteredAppointments.length === 0 ? (
                  <TableRow><TableCell colSpan={isMechanic ? 7 : 8} className="text-center py-8 text-gray-500">No hay citas programadas</TableCell></TableRow>
                ) : (
                  [...filteredAppointments]
                    .sort((a, b) => new Date(`${a.date} ${a.time}`).getTime() - new Date(`${b.date} ${b.time}`).getTime())
                    .map((apt) => {
                      const vehicle  = vehicles.find((v) => v.id === apt.vehicleId);
                      const mechanic = users.find((u) => u.id === apt.mechanicId);
                      return (
                        <TableRow key={apt.id}>
                          <TableCell>{new Date(apt.date).toLocaleDateString('es-ES')}</TableCell>
                          <TableCell className="font-medium">{apt.time}</TableCell>
                          <TableCell>{apt.clientName}</TableCell>
                          <TableCell>
                            {vehicle ? (
                              <div>
                                <p className="font-medium">{vehicle.plate}</p>
                                <p className="text-sm text-gray-500">{vehicle.brand} {vehicle.model}</p>
                              </div>
                            ) : 'N/A'}
                          </TableCell>
                          <TableCell className="max-w-xs truncate">{apt.description || 'Sin descripción'}</TableCell>
                          {!isMechanic && <TableCell>{mechanic?.name || 'Sin asignar'}</TableCell>}
                          <TableCell className="text-center">
                            <Badge
                              variant={apt.status === 'cancelled' ? 'destructive' : apt.status === 'completed' ? 'default' : apt.status === 'in-progress' ? 'secondary' : 'outline'}
                              className="rounded-full"
                            >
                              {apt.status === 'pending' ? 'Pendiente' : apt.status === 'in-progress' ? 'En Proceso' : apt.status === 'cancelled' ? 'Cancelada' : 'Completado'}
                            </Badge>
                          </TableCell>
                          {isMechanic && (
                            <TableCell className="text-center">
                              <div className="flex gap-2 justify-center items-center">
                                {apt.status === 'cancelled' ? (
                                  <Badge variant="destructive" className="rounded-full">Cancelada</Badge>
                                ) : (
                                  <>
                                    <Select value={apt.status} onValueChange={(value: any) => handleUpdateStatus(apt.id, value)}>
                                      <SelectTrigger className="rounded-lg w-[140px]"><SelectValue /></SelectTrigger>
                                      <SelectContent>
                                        <SelectItem value="pending">Pendiente</SelectItem>
                                        <SelectItem value="in-progress">En Proceso</SelectItem>
                                        <SelectItem value="completed">Completado</SelectItem>
                                      </SelectContent>
                                    </Select>
                                    <Dialog open={observationDialog === apt.id} onOpenChange={(open) => { if (!open) { setObservationDialog(null); setNewObservation(''); } }}>
                                      <DialogTrigger asChild>
                                        <Button size="sm" variant="outline" onClick={() => setObservationDialog(apt.id)} className="rounded-lg">
                                          <MessageSquare className="w-4 h-4" />
                                        </Button>
                                      </DialogTrigger>
                                      <DialogContent className="rounded-lg">
                                        <DialogHeader>
                                          <DialogTitle>Observaciones de la Cita</DialogTitle>
                                          <DialogDescription>Historial y nuevas observaciones para {apt.clientName}</DialogDescription>
                                        </DialogHeader>
                                        <div className="space-y-4 py-4">
                                          {apt.observations && apt.observations.length > 0 && (
                                            <div className="space-y-2">
                                              <Label>Observaciones Anteriores</Label>
                                              <div className="max-h-40 overflow-y-auto space-y-2">
                                                {apt.observations.map((obs, idx) => (
                                                  <div key={idx} className="p-3 bg-gray-50 rounded-lg text-sm">{obs}</div>
                                                ))}
                                              </div>
                                            </div>
                                          )}
                                          <div className="space-y-2">
                                            <Label>Nueva Observación</Label>
                                            <Textarea value={newObservation} onChange={(e) => setNewObservation(e.target.value)} placeholder="Agregar observación sobre el servicio..." rows={3} className="rounded-lg resize-none" />
                                          </div>
                                        </div>
                                        <DialogFooter>
                                          <Button onClick={() => handleAddObservation(apt.id)} disabled={isSubmitting} className="bg-[#FCA311] hover:bg-[#FCA311]/90 rounded-lg">
                                            {isSubmitting ? 'Guardando...' : 'Agregar Observación'}
                                          </Button>
                                        </DialogFooter>
                                      </DialogContent>
                                    </Dialog>
                                  </>
                                )}
                              </div>
                            </TableCell>
                          )}
                          {isReceptionist && (
                            <TableCell className="text-center">
                              <div className="flex gap-2 justify-center">
                                <Button size="sm" variant="outline" onClick={() => handleEditAppointment(apt.id)} className="rounded-lg">
                                  <Edit className="w-4 h-4 mr-1" />Editar
                                </Button>
                                {apt.status !== 'cancelled' && (
                                  <Button size="sm" variant="destructive" onClick={() => handleCancelAppointment(apt.id)} className="rounded-lg">
                                    <X className="w-4 h-4 mr-1" />Cancelar
                                  </Button>
                                )}
                              </div>
                            </TableCell>
                          )}
                        </TableRow>
                      );
                    })
                )}
              </TableBody>
            </Table>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
