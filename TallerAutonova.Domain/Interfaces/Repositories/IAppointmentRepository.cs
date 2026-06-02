using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.Interfaces.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<bool> ExistsConflictAsync(int mechanicId, DateOnly date, TimeOnly time);
        Task<IEnumerable<Appointment>> GetAllWithDetailsAsync();
        Task<Appointment?> GetByMechanicIdAsync(int mechanicId);
        Task<Appointment?> GetByIdWithVehicleAsync(int appointmentId);
        Task<IEnumerable<Appointment>> GetAllByMechanicIdAsync(int mechanicId);
        Task<IEnumerable<Appointment>> GetAllByVehiclePlateAsync(string vehiclePlate);
    }
}