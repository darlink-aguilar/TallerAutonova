using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.Interfaces.Repositories
{
    public interface IReceptionistRepository : IGenericRepository<Appointment>
    {

        // Verifica si un mecánico tiene una cita activa
        Task<bool> IsMechanicOccupiedAsync(int mechanicId, DateTime dateTime, int? excludeAppointmentId = null);

        // Obtiene todas las citas del sistema ordenadas cronológicamente por fecha y hora
        Task<IEnumerable<Appointment>> GetAllAppointmentsOrderedAsync();

        // Obtiene las citas específicas de un mecánico ordenadas por fecha y hora
        Task<IEnumerable<Appointment>> GetAppointmentsByMechanicIdAsync(int mechanicId);

        // Verifica si un vehículo existe en la base de datos por su ID
        Task<bool> VehicleExistsAsync(int vehicleId);

        // Verifica si ya existe un vehículo registrado con una placa específica
        Task<bool> PlateExistsAsync(string plate);

        // Verifica si un vehículo tiene citas pendientes/confirmadas o servicios en proceso
        Task<bool> VehicleHasActiveProcessesAsync(int vehicleId);

        // Obtiene un vehículo por su identificador único
        Task<Vehicle?> GetVehicleByIdAsync(int vehicleId);

        // Registra un nuevo vehículo en el sistema
        Task<Vehicle> CreateVehicleAsync(Vehicle vehicle);

        // Actualiza un vehículo en el sistema
        Task UpdateVehicleAsync(Vehicle vehicle);

        // Verifica si un propietario existe en el sistema.
        Task<bool> OwnerExistsAsync(int ownerId);

        // Obtiene el historial completo de mantenimientos asociado a un vehículo.
        Task<IEnumerable<MaintenanceHistory>> GetHistoryByVehicleIdAsync(int vehicleId);
    }
}