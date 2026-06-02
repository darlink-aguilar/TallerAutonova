using AutoNova.Domain.Entities;
using AutoNova.Domain.Enums;

namespace AutoNova.Domain.Interfaces.Services;

public interface IAppointmentService
{
    Task<IEnumerable<Appointment>> GetAllAsync();
    Task<Appointment> GetByIdAsync(Guid id);
    Task<IEnumerable<Appointment>> GetByVehicleIdAsync(Guid vehicleId);
    Task<IEnumerable<Appointment>> GetByMechanicIdAsync(Guid mechanicId);
    Task<Appointment> CreateAsync(DateTime date, TimeSpan time, string description, Guid vehicleId, Guid? mechanicId);
    Task<Appointment> UpdateAsync(Guid id, DateTime date, TimeSpan time, string description, Guid vehicleId, Guid? mechanicId);
    Task StartAsync(Guid id);
    Task CompleteAsync(Guid id);
    Task CancelAsync(Guid id);
}
