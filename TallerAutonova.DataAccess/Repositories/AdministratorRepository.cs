using Microsoft.EntityFrameworkCore;
using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Interfaces.Repositories;
using TallerAutonova.DataAccess.Context;

namespace TallerAutonova.DataAccess.Repositories
{
    public class AdministratorRepository : GenericRepository<Administrator>, IAdministratorRepository
    {
        public AdministratorRepository(TallerDbContext context) : base(context){ }

        // Specific queries
        public async Task<Administrator?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<IEnumerable<Administrator>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(a => a.Users)
                .Include(a => a.Parts)
                .ToListAsync();
        }
    }
}