using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task StartAsync(int appointmentId);
        Task CompleteAsync(int appointmentId);
        Task CancelAsync(int appointmentId);
        Task<Appointment?> GetByIdAsync(int appointmentId);
        Task<Appointment?> GetByMechanicIdAsync(int mechanicId);
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<IEnumerable<Appointment>> GetAllByMechanicIdAsync(int mechanicId);
        Task<IEnumerable<Appointment>> GetAllByVehiclePlateAsync(string vehiclePlate);
        Task<Appointment> CreateAsync(Appointment appointment);
        Task UpdateAsync(int appointmentId, Appointment appointment);
        Task DeleteAsync(int appointmentId);
    }
}