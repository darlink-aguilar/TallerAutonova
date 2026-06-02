using AutoNova.Domain.Entities;

namespace AutoNova.Domain.Interfaces.Repositories;

public interface IMaintenanceHistoryRepository
{
    Task<MaintenanceHistory?> GetByIdAsync(Guid id);
    Task<IEnumerable<MaintenanceHistory>> GetAllAsync();
    Task<IEnumerable<MaintenanceHistory>> GetByVehicleIdAsync(Guid vehicleId);
    Task<IEnumerable<MaintenanceHistory>> GetByPlateAsync(string plate);
    Task AddAsync(MaintenanceHistory history);
}
