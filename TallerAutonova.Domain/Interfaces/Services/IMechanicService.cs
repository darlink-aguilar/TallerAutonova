using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.Interfaces.Services
{
    public interface IMechanicService
    {
        Task<MaintenanceHistory> AddObservationAsync(MaintenanceHistory maintenanceHistory); // Registra una nueva observación de mantenimiento en el historial de un vehículo
        Task<IEnumerable<MaintenanceHistory>> GetHistoryByVehicleIdAsync(int vehicleId); // Consulta el historial completo de mantenimientos de un vehículo específico
        Task<IEnumerable<Appointment>> GetAppointmentsByMechanicIdAsync(int mechanicId); // Consulta las citas programadas para un mecánico.
    }
}