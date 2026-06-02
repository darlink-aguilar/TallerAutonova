using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TallerAutonova.DataAccess.Context;
using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Enums;
using TallerAutonova.Domain.Interfaces.Repositories;

namespace TallerAutonova.DataAccess.Repositories
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehiclesRepository
    {
        public VehicleRepository(TallerDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsWithPlateAsync(string plate)
        {
            return await _dbSet
                .AnyAsync(a => a.Plate == plate);
        }

        public async Task<Vehicle?> GetByIdWithOwnerAsync(int vehicleId)
        {
            return await _dbSet
                .Where(v => v.Id == vehicleId)
                .Include(v => v.Owner)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetAllWithOwnerAsync()
        {
            return await _dbSet
                .Include(v => v.Owner)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetByOwnerNameAsync(string owner)
        {
            return await _dbSet
                .Where(v => v.Owner.Name == owner)
                .Include(v => v.Owner)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetByBrandIdAsync(string brand)
        {
            return await _dbSet
                .Where(v => v.Brand == brand)
                .Include(v => v.Owner)
                .ToListAsync();
        }
    }
}