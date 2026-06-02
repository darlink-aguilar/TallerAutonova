import { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { Card, CardContent, CardHeader, CardTitle } from '../components/ui/card';
import { Input } from '../components/ui/input';
import { Button } from '../components/ui/button';
import {
  Table, TableBody, TableCell, TableHead, TableHeader, TableRow,
} from '../components/ui/table';
import { Search, Plus, UserX, UserCheck } from 'lucide-react';
import { Badge } from '../components/ui/badge';
import {
  Dialog, DialogContent, DialogDescription, DialogFooter,
  DialogHeader, DialogTitle, DialogTrigger,
} from '../components/ui/dialog';
import {
  Select, SelectContent, SelectItem, SelectTrigger, SelectValue,
} from '../components/ui/select';
import { Label } from '../components/ui/label';
import { toast } from 'sonner';
import type { UserRole } from '../context/AuthContext';

export function UserManagement() {
  const { users, addUser, updateUser } = useAuth();
  const [searchTerm, setSearchTerm]   = useState('');
  const [isAddDialogOpen, setIsAddDialogOpen] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const [formData, setFormData] = useState({
    name: '', email: '', password: '', role: 'mechanic' as UserRole,
  });

  const filteredUsers = users.filter(
    (u) =>
      u.role !== 'admin' &&
      (u.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        u.email.toLowerCase().includes(searchTerm.toLowerCase()))
  );

  const handleAddUser = async () => {
    if (!formData.name || !formData.email || !formData.password) {
      toast.error('Por favor complete todos los campos');
      return;
    }
    setIsSubmitting(true);
    try {
      await addUser(formData.name, formData.email, formData.password, formData.role);
      toast.success('Usuario creado exitosamente');
      setIsAddDialogOpen(false);
      setFormData({ name: '', email: '', password: '', role: 'mechanic' });
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Error al crear el usuario');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleToggleUserStatus = async (id: string, currentStatus: boolean) => {
    try {
      await updateUser(id, { isActive: !currentStatus });
      toast.success(currentStatus ? 'Usuario desactivado' : 'Usuario activado');
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Error al cambiar el estado');
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h2 className="text-3xl font-bold text-[#0B2545]">Gestión de Usuarios</h2>
          <p className="text-gray-600 mt-1">Administración de empleados del sistema</p>
        </div>
        <Dialog open={isAddDialogOpen} onOpenChange={setIsAddDialogOpen}>
          <DialogTrigger asChild>
            <Button className="bg-[#FCA311] hover:bg-[#FCA311]/90 rounded-lg">
              <Plus className="w-4 h-4 mr-2" />
              Crear Usuario
            </Button>
          </DialogTrigger>
          <DialogContent className="rounded-lg">
            <DialogHeader>
              <DialogTitle>Crear Nuevo Usuario</DialogTitle>
              <DialogDescription>Registre un nuevo empleado en el sistema</DialogDescription>
            </DialogHeader>
            <div className="space-y-4 py-4">
              <div className="space-y-2">
                <Label>Nombre Completo *</Label>
                <Input value={formData.name} onChange={(e) => setFormData({ ...formData, name: e.target.value })} placeholder="Ej: Juan Pérez" className="rounded-lg" />
              </div>
              <div className="space-y-2">
                <Label>Correo Electrónico *</Label>
                <Input type="email" value={formData.email} onChange={(e) => setFormData({ ...formData, email: e.target.value })} placeholder="correo@autonova.com" className="rounded-lg" />
              </div>
              <div className="space-y-2">
                <Label>Contraseña *</Label>
                <Input type="password" value={formData.password} onChange={(e) => setFormData({ ...formData, password: e.target.value })} placeholder="••••••••" className="rounded-lg" />
              </div>
              <div className="space-y-2">
                <Label>Rol *</Label>
                <Select value={formData.role} onValueChange={(v: UserRole) => setFormData({ ...formData, role: v })}>
                  <SelectTrigger className="rounded-lg"><SelectValue /></SelectTrigger>
                  <SelectContent>
                    <SelectItem value="mechanic">Mecánico</SelectItem>
                    <SelectItem value="receptionist">Recepcionista</SelectItem>
                  </SelectContent>
                </Select>
              </div>
            </div>
            <DialogFooter>
              <Button onClick={handleAddUser} disabled={isSubmitting} className="bg-[#FCA311] hover:bg-[#FCA311]/90 rounded-lg">
                {isSubmitting ? 'Guardando...' : 'Guardar Usuario'}
              </Button>
            </DialogFooter>
          </DialogContent>
        </Dialog>
      </div>

      <Card className="rounded-lg shadow-md">
        <CardContent className="pt-6">
          <div className="relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 w-5 h-5" />
            <Input placeholder="Buscar por nombre o correo..." value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} className="pl-10 rounded-lg" />
          </div>
        </CardContent>
      </Card>

      <Card className="rounded-lg shadow-md">
        <CardHeader><CardTitle>Lista de Empleados</CardTitle></CardHeader>
        <CardContent>
          <div className="rounded-lg border">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Nombre</TableHead><TableHead>Correo</TableHead>
                  <TableHead className="text-center">Rol</TableHead>
                  <TableHead className="text-center">Estado</TableHead>
                  <TableHead className="text-center">Acciones</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {filteredUsers.length === 0 ? (
                  <TableRow><TableCell colSpan={5} className="text-center py-8 text-gray-500">No se encontraron usuarios</TableCell></TableRow>
                ) : (
                  filteredUsers.map((u) => (
                    <TableRow key={u.id}>
                      <TableCell className="font-medium">{u.name}</TableCell>
                      <TableCell>{u.email}</TableCell>
                      <TableCell className="text-center">
                        <Badge variant="outline" className="rounded-full">
                          {u.role === 'mechanic' ? 'Mecánico' : 'Recepcionista'}
                        </Badge>
                      </TableCell>
                      <TableCell className="text-center">
                        <Badge variant={u.isActive ? 'default' : 'secondary'} className="rounded-full">
                          {u.isActive ? 'Activo' : 'Inactivo'}
                        </Badge>
                      </TableCell>
                      <TableCell className="text-center">
                        <Button size="sm" variant={u.isActive ? 'destructive' : 'default'} className="rounded-lg" onClick={() => handleToggleUserStatus(u.id, u.isActive)}>
                          {u.isActive ? <><UserX className="w-4 h-4 mr-1" />Desactivar</> : <><UserCheck className="w-4 h-4 mr-1" />Activar</>}
                        </Button>
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
