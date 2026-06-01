using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.Interfaces.Services
{
    public interface IReceptionistService
    {
    
        // Agenda una nueva cita en el sistema
        Task<Appointment> ScheduleAppointmentAsync(Appointment appointment);

        // Reagenda una cita existente modificando su fecha u hora
        Task RescheduleAppointmentAsync(int id, DateTime newDateTime);

        // Cancela una cita de forma lógica cambiando su estado
        Task CancelAppointmentAsync(int id);

        // Obtiene todas las citas del sistema ordenadas por fecha y hora
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();

        // Obtiene las citas asignadas a un mecánico específico (Para uso de Mecánicos)
        Task<IEnumerable<Appointment>> GetAppointmentsByMechanicIdAsync(int mechanicId);

        // Añade o modifica la descripción de notas de una cita existente (Máx 300 caracteres)
        Task AddAppointmentDescriptionAsync(int id, string description);

        // Registra un nuevo vehículo en el sistema verificando que la placa no esté duplicada
        Task<Vehicle> RegisterVehicleAsync(Vehicle vehicle);

        // Desactiva de forma lógica un vehículo si no posee procesos activos ni citas pendientes
        Task DeactivateVehicleAsync(int vehicleId);

        // Actualiza los datos del vehículo y del propietario en el sistema
        Task UpdateVehicleDataAsync(int vehicleId, Vehicle vehicle);

        // Consulta el historial completo de intervenciones mecánicas de un vehículo
        Task<IEnumerable<MaintenanceHistory>> GetVehicleHistoryAsync(int vehicleId);
    }
}