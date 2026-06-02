using Microsoft.EntityFrameworkCore;
using TallerAutonova.DataAccess.Context;
using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Enums;
using TallerAutonova.Domain.Interfaces.Repositories;

namespace TallerAutonova.DataAccess.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(TallerDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsConflictAsync(int mechanicId, DateOnly date, TimeOnly time)
        {
            return await _dbSet
                .AnyAsync(a =>
                    a.MechanicId == mechanicId
                    && a.Date.Equals(date)
                    && a.Time.Equals(time)
                    && a.State != AppointmentStatus.Cancelled);
        }

        public async Task<IEnumerable<Appointment>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(m => m.Vehicle)
                    .ThenInclude(v => v.Owner)
                .Include(m => m.Mechanic)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByMechanicIdAsync(int mechanicId)
        {
            return await _dbSet
                .Where(m => m.MechanicId == mechanicId)
                .Include(m => m.Vehicle)
                    .ThenInclude(v => v.Owner)
                .FirstOrDefaultAsync();
        }
        public async Task<Appointment?> GetByIdWithVehicleAsync(int appointmentId)
        {
            return await _dbSet
                .Where(m => m.Id == appointmentId)
                .Include(m => m.Vehicle)
                    .ThenInclude(v => v.Owner)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllByMechanicIdAsync(int mechanicId)
        {
            return await _dbSet
                .Where(mh => mh.MechanicId == mechanicId)
                .Include(m => m.Vehicle)
                    .ThenInclude(v => v.Owner)
                .OrderByDescending(mh => mh.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllByVehiclePlateAsync(string vehiclePlate)
        {
            return await _context.Set<Appointment>()
                .Where(a => a.Vehicle.Plate == vehiclePlate)
                .Include(m => m.Vehicle)
                    .ThenInclude(v => v.Owner)
                .OrderBy(a => a.Date)
                .ToListAsync();
        }
    }
}