using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.Interfaces.Repositories
{
    public interface IPartRepository : IGenericRepository<Part>
    {
        // Specific queries
        Task<IEnumerable<Part>> GetLowStockPartsAsync();
        Task<IEnumerable<Part>> GetByAdministratorIdAsync(int administratorId);
    }
}