using AutoNova.Domain.Entities;

namespace AutoNova.Domain.Interfaces.Repositories;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByIdAsync(Guid id);
    Task<Vehicle?> GetByPlateAsync(string plate);
    Task<IEnumerable<Vehicle>> GetAllAsync();
    Task AddAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle);
    Task<bool> ExistsByPlateAsync(string plate);
}
