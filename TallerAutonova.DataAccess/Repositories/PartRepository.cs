using Microsoft.EntityFrameworkCore;
using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Interfaces.Repositories;
using TallerAutonova.DataAccess.Context;

namespace TallerAutonova.DataAccess.Repositories
{
    public class PartRepository : GenericRepository<Part>, IPartRepository
    {
        public PartRepository(TallerDbContext context) : base(context){ }

        // Specific queries
        public async Task<IEnumerable<Part>> GetLowStockPartsAsync()
        {
            return await _dbSet.Where(p => p.Quantity <= p.MinStock).ToListAsync();
        }

        public async Task<IEnumerable<Part>> GetByAdministratorIdAsync(int administratorId)
        {
            return await _dbSet.Where(p => p.AdministratorId == administratorId).ToListAsync();
        }

        // Stock management
        public async Task UpdateStockAsync(int partId, int newQuantity)
        {
            var part = await GetByIdAsync(partId);
            if (part != null)
            {
                part.Quantity = newQuantity;
                part.UpdatedAt = DateTime.UtcNow;
                await UpdateAsync(part);
            }
        }

        public async Task<bool> HasStockAvailableAsync(int partId, int requestedQuantity)
        {
            var part = await GetByIdAsync(partId);
            return part != null && part.Quantity >= requestedQuantity;
        }
    }
}