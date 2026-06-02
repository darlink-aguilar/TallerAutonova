using AutoNova.Domain.Entities;

namespace AutoNova.Domain.Interfaces.Services;

public interface IVehicleService
{
    Task<IEnumerable<Vehicle>> GetAllAsync();
    Task<Vehicle> GetByIdAsync(Guid id);
    Task<Vehicle> GetByPlateAsync(string plate);
    Task<Vehicle> CreateAsync(Vehicle vehicle, Owner owner);
    Task<Vehicle> UpdateAsync(Guid id, string plate, string brand, string model, int year, string color);
    Task DeactivateAsync(Guid id);
    Task ActivateAsync(Guid id);
}
