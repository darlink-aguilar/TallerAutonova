using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.Interfaces.Repositories
{
    public interface IVehiclesRepository : IGenericRepository<Vehicle>
    {
        Task<bool> ExistsWithPlateAsync(string plate);
        Task<Vehicle?> GetByIdWithOwnerAsync(int vehicleId);
        Task<IEnumerable<Vehicle>> GetAllWithOwnerAsync();
        Task<IEnumerable<Vehicle>> GetByOwnerNameAsync(string owner);
        Task<IEnumerable<Vehicle>> GetByBrandIdAsync(string brand);
    }
}