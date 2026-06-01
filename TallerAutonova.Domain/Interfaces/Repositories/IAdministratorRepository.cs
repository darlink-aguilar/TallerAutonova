using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.Interfaces.Repositories
{
    public interface IAdministratorRepository : IGenericRepository<Administrator>
    {
        // Specific queries
        Task<Administrator?> GetByEmailAsync(string email);
        Task<IEnumerable<Administrator>> GetAllWithDetailsAsync();
    }
}