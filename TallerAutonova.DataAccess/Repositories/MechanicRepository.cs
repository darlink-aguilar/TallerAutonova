using Microsoft.EntityFrameworkCore;
using TallerAutonova.DataAccess.Context; 
using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Interfaces.Repositories;

namespace TallerAutonova.DataAccess.Repositories
{
    public class MechanicRepository : GenericRepository<MaintenanceHistory>, IMechanicRepository
    {
        public MechanicRepository(TallerDbContext context) : base(context)
        {
        }

        public async Task<bool> VehicleExistsAsync(int vehicleId)
        {
            return await _context.Set<Vehicle>()
                .AnyAsync(v => v.Id == vehicleId); 
        }

        public async Task<IEnumerable<MaintenanceHistory>> GetHistoryByVehicleIdAsync(int vehicleId)
        {
            return await _dbSet
                .Where(mh => mh.VehicleId == vehicleId)
                .OrderByDescending(mh => mh.Date) 
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByMechanicIdAsync(int mechanicId)
        {
            return await _context.Set<Appointment>()
                .Where(a => a.MechanicId == mechanicId)
                .OrderBy(a => a.Date) 
                .ToListAsync();
        }
    }
}