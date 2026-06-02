using Microsoft.EntityFrameworkCore;
using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Interfaces.Repositories;
using TallerAutonova.DataAccess.Context;

namespace TallerAutonova.DataAccess.Repositories
{
    public class PartRepository : GenericRepository<Part>, IPartRepository
    {
        public PartRepository(TallerDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Part>> GetLowStockPartsAsync()
        {
            return await _dbSet.Where(p => p.Quantity <= p.MinStock).ToListAsync();
        }

        public async Task<IEnumerable<Part>> GetByAdministratorIdAsync(int administratorId)
        {
            return await _dbSet.Where(p => p.AdministratorId == administratorId).ToListAsync();
        }
    }
}