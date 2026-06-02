using AutoNova.Domain.Entities;

namespace AutoNova.Domain.Interfaces.Services;

public interface IMaintenanceHistoryService
{
    Task<IEnumerable<MaintenanceHistory>> GetAllAsync();
    Task<MaintenanceHistory> GetByIdAsync(Guid id);
    Task<IEnumerable<MaintenanceHistory>> GetByVehicleIdAsync(Guid vehicleId);
    Task<IEnumerable<MaintenanceHistory>> GetByPlateAsync(string plate);
    Task<MaintenanceHistory> AddAsync(Guid vehicleId, string observation, string servicePerformed, Guid mechanicId);
}
