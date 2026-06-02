using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.Interfaces.Services
{
    public interface IVehicleService
    {
        Task<Vehicle?> GetByIdAsync(int VehicleId);
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<IEnumerable<Vehicle>> GetAllByOwnerAsync(string owner);
        Task<IEnumerable<Vehicle>> GetAllByBrandAsync(string brand);
        Task<Vehicle?> CreateAsync(Vehicle Vehicle, string ownerName, string ownerPhone);
        Task UpdateAsync(int id, Vehicle Vehicle);
        Task UpdateOwnerAsync(int id, Owner owner);
        Task DeleteAsync(int VehicleId);
    }
}
