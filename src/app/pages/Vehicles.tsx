import { useState } from 'react';
import { useData } from '../context/DataContext';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router';
import { Card, CardContent, CardHeader, CardTitle } from '../components/ui/card';
import { Input } from '../components/ui/input';
import { Button } from '../components/ui/button';
import {
  Table, TableBody, TableCell, TableHead, TableHeader, TableRow,
} from '../components/ui/table';
import { Search, Plus, Eye, Trash2, CheckCircle, Edit } from 'lucide-react';
import { Badge } from '../components/ui/badge';
import {
  Dialog, DialogContent, DialogDescription, DialogFooter,
  DialogHeader, DialogTitle, DialogTrigger,
} from '../components/ui/dialog';
import {
  AlertDialog, AlertDialogAction, AlertDialogCancel, AlertDialogContent,
  AlertDialogDescription, AlertDialogFooter, AlertDialogHeader, AlertDialogTitle,
  AlertDialogTrigger,
} from '../components/ui/alert-dialog';
import { Label } from '../components/ui/label';
import { toast } from 'sonner';

export function Vehicles() {
  const { vehicles, addVehicle, updateVehicle, appointments } = useData();
  const { user } = useAuth();
  const navigate  = useNavigate();

  const isReceptionist = user?.role === 'receptionist';
  const [searchTerm, setSearchTerm]           = useState('');
  const [isAddDialogOpen, setIsAddDialogOpen] = useState(false);
  const [editingVehicle, setEditingVehicle]   = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting]       = useState(false);

  const [formData, setFormData] = useState({
    plate: '', brand: '', model: '', year: new Date().getFullYear(),
    color: '', ownerName: '', ownerDocumentNumber: '', ownerPhone: '', ownerEmail: '',
  });
  const [editFormData, setEditFormData] = useState({ ownerName: '', ownerPhone: '' });

  const filteredVehicles = vehicles.filter(
    (v) =>
      v.plate.toLowerCase().includes(searchTerm.toLowerCase()) ||
      v.ownerName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      v.brand.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const handleAddVehicle = async () => {
    if (!formData.plate || !formData.brand || !formData.model || !formData.ownerName || !formData.ownerDocumentNumber) {
      toast.error('Placa, marca, modelo, nombre y documento del propietario son obligatorios');
      return;
    }
    setIsSubmitting(true);
    try {
      await addVehicle({
        plate: formData.plate, brand: formData.brand, model: formData.model,
        year: formData.year, color: formData.color, ownerName: formData.ownerName,
        ownerDocumentNumber: formData.ownerDocumentNumber, ownerPhone: formData.ownerPhone,
        ownerEmail: formData.ownerEmail, isActive: true,
      });
      toast.success('Vehículo registrado exitosamente');
      setIsAddDialogOpen(false);
      setFormData({ plate: '', brand: '', model: '', year: new Date().getFullYear(), color: '', ownerName: '', ownerDocumentNumber: '', ownerPhone: '', ownerEmail: '' });
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Error al registrar el vehículo');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleDeactivateVehicle = async (id: string) => {
    const hasActive = appointments.some(
      (a) => a.vehicleId === id && (a.status === 'pending' || a.status === 'in-progress')
    );
    if (hasActive) {
      toast.error('No se puede desactivar. El vehículo tiene citas activas o en proceso');
      return;
    }
    try {
      await updateVehicle(id, { isActive: false });
      toast.success('Vehículo marcado como inactivo');
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Error al desactivar');
    }
  };

  const handleEditVehicle = (id: string) => {
    const v = vehicles.find((x) => x.id === id);
    if (v) { setEditingVehicle(id); setEditFormData({ ownerName: v.ownerName, ownerPhone: v.ownerPhone }); }
  };

  const handleSaveEdit = async (id: string) => {
    setIsSubmitting(true);
    try {
      await updateVehicle(id, { ownerName: editFormData.ownerName, ownerPhone: editFormData.ownerPhone });
      toast.success('Vehículo actualizado exitosamente');
      setEditingVehicle(null);
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Error al actualizar');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h2 className="text-3xl font-bold text-[#0B2545]">Gestión de Vehículos</h2>
          <p className="text-gray-600 mt-1">Registro y consulta de vehículos</p>
        </div>
        {isReceptionist && (
          <Dialog open={isAddDialogOpen} onOpenChange={setIsAddDialogOpen}>
            <DialogTrigger asChild>
              <Button className="bg-[#FCA311] hover:bg-[#FCA311]/90 rounded-lg">
                <Plus className="w-4 h-4 mr-2" />Registrar Vehículo
              </Button>
            </DialogTrigger>
            <DialogContent className="rounded-lg max-w-2xl">
              <DialogHeader>
                <DialogTitle>Registrar Nuevo Vehículo</DialogTitle>
                <DialogDescription>Complete la información del vehículo y su propietario</DialogDescription>
              </DialogHeader>
              <div className="grid grid-cols-2 gap-4 py-4">
                <div className="space-y-2"><Label>Placa *</Label><Input value={formData.plate} onChange={(e) => setFormData({ ...formData, plate: e.target.value })} placeholder="ABC123" className="rounded-lg" /></div>
                <div className="space-y-2"><Label>Marca *</Label><Input value={formData.brand} onChange={(e) => setFormData({ ...formData, brand: e.target.value })} placeholder="Toyota, Honda..." className="rounded-lg" /></div>
                <div className="space-y-2"><Label>Modelo *</Label><Input value={formData.model} onChange={(e) => setFormData({ ...formData, model: e.target.value })} placeholder="Corolla, Civic..." className="rounded-lg" /></div>
                <div className="space-y-2"><Label>Año</Label><Input type="number" value={formData.year} onChange={(e) => setFormData({ ...formData, year: parseInt(e.target.value) })} className="rounded-lg" /></div>
                <div className="space-y-2"><Label>Color</Label><Input value={formData.color} onChange={(e) => setFormData({ ...formData, color: e.target.value })} placeholder="Rojo, Azul..." className="rounded-lg" /></div>
                <div className="space-y-2"><Label>Nombre Propietario *</Label><Input value={formData.ownerName} onChange={(e) => setFormData({ ...formData, ownerName: e.target.value })} placeholder="Nombre completo" className="rounded-lg" /></div>
                <div className="space-y-2"><Label>Documento Propietario *</Label><Input value={formData.ownerDocumentNumber} onChange={(e) => setFormData({ ...formData, ownerDocumentNumber: e.target.value })} placeholder="Cédula o documento" className="rounded-lg" /></div>
                <div className="space-y-2"><Label>Teléfono</Label><Input value={formData.ownerPhone} onChange={(e) => setFormData({ ...formData, ownerPhone: e.target.value })} placeholder="555-1234" className="rounded-lg" /></div>
                <div className="space-y-2 col-span-2"><Label>Correo Propietario</Label><Input type="email" value={formData.ownerEmail} onChange={(e) => setFormData({ ...formData, ownerEmail: e.target.value })} placeholder="propietario@email.com" className="rounded-lg" /></div>
              </div>
              <DialogFooter>
                <Button onClick={handleAddVehicle} disabled={isSubmitting} className="bg-[#FCA311] hover:bg-[#FCA311]/90 rounded-lg">
                  {isSubmitting ? 'Guardando...' : 'Guardar Vehículo'}
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
            <Input placeholder="Buscar por placa, propietario o marca..." value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} className="pl-10 rounded-lg" />
          </div>
        </CardContent>
      </Card>

      <Card className="rounded-lg shadow-md">
        <CardHeader><CardTitle>Lista de Vehículos</CardTitle></CardHeader>
        <CardContent>
          <div className="rounded-lg border">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Placa</TableHead><TableHead>Marca/Modelo</TableHead>
                  <TableHead>Año</TableHead><TableHead>Propietario</TableHead>
                  <TableHead>Teléfono</TableHead><TableHead className="text-center">Estado</TableHead>
                  <TableHead className="text-center">Acciones</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {filteredVehicles.length === 0 ? (
                  <TableRow><TableCell colSpan={7} className="text-center py-8 text-gray-500">No se encontraron vehículos</TableCell></TableRow>
                ) : (
                  filteredVehicles.map((vehicle) => (
                    <TableRow key={vehicle.id}>
                      <TableCell className="font-medium">{vehicle.plate}</TableCell>
                      <TableCell>{vehicle.brand} {vehicle.model}</TableCell>
                      <TableCell>{vehicle.year}</TableCell>
                      <TableCell>
                        {editingVehicle === vehicle.id
                          ? <Input value={editFormData.ownerName} onChange={(e) => setEditFormData({ ...editFormData, ownerName: e.target.value })} className="rounded-lg" />
                          : vehicle.ownerName}
                      </TableCell>
                      <TableCell>
                        {editingVehicle === vehicle.id
                          ? <Input value={editFormData.ownerPhone} onChange={(e) => setEditFormData({ ...editFormData, ownerPhone: e.target.value })} className="rounded-lg" />
                          : vehicle.ownerPhone}
                      </TableCell>
                      <TableCell className="text-center">
                        <Badge variant={vehicle.isActive ? 'default' : 'secondary'} className="rounded-full">
                          {vehicle.isActive ? 'Activo' : 'Inactivo'}
                        </Badge>
                      </TableCell>
                      <TableCell className="text-center">
                        <div className="flex items-center justify-center gap-2">
                          <Button size="sm" variant="outline" className="rounded-lg" onClick={() => navigate(`/dashboard/vehicles/${vehicle.id}`)}>
                            <Eye className="w-4 h-4 mr-1" />Ver Historial
                          </Button>
                          {isReceptionist && !vehicle.isActive && (
                            <Button size="sm" variant="outline" className="rounded-lg bg-green-50 hover:bg-green-100 text-green-700"
                              onClick={async () => { try { await updateVehicle(vehicle.id, { isActive: true }); toast.success('Vehículo activado'); } catch { toast.error('Error al activar'); } }}>
                              <CheckCircle className="w-4 h-4 mr-1" />Activar
                            </Button>
                          )}
                          {isReceptionist && vehicle.isActive && (
                            <AlertDialog>
                              <AlertDialogTrigger asChild>
                                <Button size="sm" variant="destructive" className="rounded-lg"><Trash2 className="w-4 h-4" /></Button>
                              </AlertDialogTrigger>
                              <AlertDialogContent className="rounded-lg">
                                <AlertDialogHeader>
                                  <AlertDialogTitle>Confirmar Desactivación</AlertDialogTitle>
                                  <AlertDialogDescription>¿Desea marcar este vehículo como inactivo? Esta acción se puede revertir.</AlertDialogDescription>
                                </AlertDialogHeader>
                                <AlertDialogFooter>
                                  <AlertDialogCancel className="rounded-lg">Cancelar</AlertDialogCancel>
                                  <AlertDialogAction onClick={() => handleDeactivateVehicle(vehicle.id)} className="bg-red-600 hover:bg-red-700 rounded-lg">Confirmar</AlertDialogAction>
                                </AlertDialogFooter>
                              </AlertDialogContent>
                            </AlertDialog>
                          )}
                          {isReceptionist && editingVehicle !== vehicle.id && (
                            <Button size="sm" variant="outline" className="rounded-lg" onClick={() => handleEditVehicle(vehicle.id)}>
                              <Edit className="w-4 h-4" />
                            </Button>
                          )}
                          {isReceptionist && editingVehicle === vehicle.id && (
                            <Button size="sm" variant="outline" className="rounded-lg bg-green-50 hover:bg-green-100" disabled={isSubmitting} onClick={() => handleSaveEdit(vehicle.id)}>
                              <CheckCircle className="w-4 h-4" />
                            </Button>
                          )}
                        </div>
                      </TableCell>
                    </TableRow>
                  ))
                )}
              </TableBody>
            </Table>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
