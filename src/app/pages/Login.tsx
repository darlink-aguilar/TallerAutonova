import { useState } from 'react';
import { useNavigate } from 'react-router';
import { useAuth } from '../context/AuthContext';
import { useData } from '../context/DataContext';
import { Button } from '../components/ui/button';
import { Input } from '../components/ui/input';
import { Label } from '../components/ui/label';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../components/ui/card';
import { Settings, AlertCircle, Loader2 } from 'lucide-react';
import { Alert, AlertDescription } from '../components/ui/alert';

export function Login() {
  const [email, setEmail]       = useState('');
  const [password, setPassword] = useState('');
  const [error, setError]       = useState('');
  const [loading, setLoading]   = useState(false);

  const { login, user } = useAuth();
  const { refreshAll }  = useData();
  const navigate        = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const success = await login(email, password);
      if (success) {
        await refreshAll();
        // user is set after login; re-read from context isn't instant — use stored data
        const saved = localStorage.getItem('autonova_current_user');
        const role  = saved ? JSON.parse(saved).role : '';
        if (role === 'admin')        navigate('/dashboard/admin');
        else if (role === 'mechanic')     navigate('/dashboard/mechanic');
        else if (role === 'receptionist') navigate('/dashboard/receptionist');
      } else {
        setError('Usuario o contraseña incorrectos');
      }
    } catch {
      setError('Error de conexión con el servidor');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-[#0B2545] to-[#13315C] p-4">
      <Card className="w-full max-w-md shadow-2xl">
        <CardHeader className="space-y-4 text-center">
          <div className="flex justify-center">
            <div className="w-16 h-16 bg-[#FCA311] rounded-full flex items-center justify-center">
              <Settings className="w-8 h-8 text-white" />
            </div>
          </div>
          <CardTitle className="text-3xl">TALLER AUTONOVA</CardTitle>
          <CardDescription>Sistema de Gestión Automotriz</CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit} className="space-y-4">
            {error && (
              <Alert variant="destructive">
                <AlertCircle className="h-4 w-4" />
                <AlertDescription>{error}</AlertDescription>
              </Alert>
            )}

            <div className="space-y-2">
              <Label htmlFor="email">Correo electrónico</Label>
              <Input
                id="email"
                type="email"
                placeholder="correo@autonova.com"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
                className="rounded-lg"
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="password">Contraseña</Label>
              <Input
                id="password"
                type="password"
                placeholder="Ingrese su contraseña"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
                className="rounded-lg"
              />
            </div>

            <Button
              type="submit"
              disabled={loading}
              className="w-full rounded-lg bg-[#FCA311] hover:bg-[#FCA311]/90"
            >
              {loading ? (
                <>
                  <Loader2 className="w-4 h-4 mr-2 animate-spin" />
                  Iniciando sesión...
                </>
              ) : (
                'Iniciar sesión'
              )}
            </Button>
          </form>
        </CardContent>
      </Card>
    </div>
  );
}
