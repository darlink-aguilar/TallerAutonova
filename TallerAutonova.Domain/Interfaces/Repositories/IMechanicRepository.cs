using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.Interfaces.Repositories
{
    public interface IMechanicRepository : IGenericRepository<MaintenanceHistory>
    {
        Task<bool> VehicleExistsAsync(int vehicleId); // Verifica si un vehículo existe en la base de datos por su ID.
        Task<IEnumerable<MaintenanceHistory>> GetHistoryByVehicleIdAsync(int vehicleId); // Obtiene todos los registros de mantenimiento asociados a un vehículo
        Task<IEnumerable<Appointment>> GetAppointmentsByMechanicIdAsync(int mechanicId); // Obtiene las citas asignadas a un mecánico específico.
    }
}