using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.Services.Observer
{
    public class AlertaAdmin : IStockObserver
    {
        public void actualizar(Part repuesto)
        {
            string mensaje = $@"
╔══════════════════════════════════════════════════════════════╗
║                    ⚠️ ALERTA DE STOCK BAJO ⚠️                 ║
╠══════════════════════════════════════════════════════════════╣
║  Repuesto: {repuesto.Name,-45} ║
║  Stock actual: {repuesto.Quantity,-39} ║
║  Stock mínimo: {repuesto.MinStock,-39} ║
║  Administrador ID: {repuesto.AdministratorId,-36} ║
╚══════════════════════════════════════════════════════════════╝";

            Console.WriteLine(mensaje);
        }
    }
}