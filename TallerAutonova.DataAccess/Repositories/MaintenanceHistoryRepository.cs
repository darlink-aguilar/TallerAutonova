using Microsoft.EntityFrameworkCore;
using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Interfaces.Repositories;
using TallerAutonova.DataAccess.Context;

namespace TallerAutonova.DataAccess.Repositories
{
    public class MaintenanceHistoryRepository : GenericRepository<MaintenanceHistory>, IMaintenanceHistoryRepository
    {
        public MaintenanceHistoryRepository(TallerDbContext context) : base(context)
        {
        }

        // Specific queries
        public async Task<IEnumerable<MaintenanceHistory>> GetByVehicleIdAsync(int vehicleId)
        {
            return await _dbSet.Where(h => h.VehicleId == vehicleId).OrderByDescending(h => h.Date).ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceHistory>> GetByMechanicIdAsync(int mechanicId)
        {
            return await _dbSet.Where(h => h.MechanicId == mechanicId).OrderByDescending(h => h.Date).ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceHistory>> GetByReceptionistIdAsync(int receptionistId)
        {
            return await _dbSet.Where(h => h.ReceptionistId == receptionistId).OrderByDescending(h => h.Date).ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceHistory>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(h => h.Date >= startDate && h.Date <= endDate).OrderBy(h => h.Date).ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceHistory>> GetRecentAsync(int count)
        {
            return await _dbSet.OrderByDescending(h => h.Date).Take(count).ToListAsync();
        }
    }
}