using AutoNova.Domain.Entities;

namespace AutoNova.Domain.Interfaces.Repositories;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(Guid id);
    Task<IEnumerable<Appointment>> GetAllAsync();
    Task<IEnumerable<Appointment>> GetByVehicleIdAsync(Guid vehicleId);
    Task<IEnumerable<Appointment>> GetByMechanicIdAsync(Guid mechanicId);
    Task<bool> ExistsByDateAndTimeAsync(DateTime date, TimeSpan time, Guid? excludeId = null);
    Task AddAsync(Appointment appointment);
    Task UpdateAsync(Appointment appointment);
}
