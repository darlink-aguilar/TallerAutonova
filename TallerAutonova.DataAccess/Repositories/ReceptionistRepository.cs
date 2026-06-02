using Microsoft.EntityFrameworkCore;
using TallerAutonova.DataAccess.Context; 
using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Enums;
using TallerAutonova.Domain.Interfaces.Repositories;

namespace TallerAutonova.DataAccess.Repositories
{
    public class ReceptionistRepository : GenericRepository<Appointment>, IReceptionistRepository
    {
        public ReceptionistRepository(TallerDbContext context) : base(context)
        {
        }


        public async Task<bool> IsMechanicOccupiedAsync(int mechanicId, DateOnly date, TimeOnly time, int? excludeAppointmentId = null)
        {
            return await _dbSet.AnyAsync(a =>
                a.MechanicId == mechanicId &&
                a.Date == date &&
                a.Time == time &&
                a.Status != AppointmentStatus.Canceled && 
                a.Status != AppointmentStatus.Completed && 
                (!excludeAppointmentId.HasValue || a.Id != excludeAppointmentId.Value));
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsOrderedAsync()
        {
            // Retorna todas las citas ordenadas cronológicamente por fecha y hora
            return await _dbSet
                .OrderBy(a => a.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByMechanicIdAsync(int mechanicId)
        {
            // Retorna las citas de un mecánico específico ordenadas cronológicamente
            return await _dbSet
                .Where(a => a.MechanicId == mechanicId)
                .OrderBy(a => a.Date)
                .ToListAsync();
        }

        public async Task<bool> VehicleExistsAsync(int vehicleId)
        {
            return await _context.Set<Vehicle>()
                .AnyAsync(v => v.Id == vehicleId);
        }

        public async Task<bool> PlateExistsAsync(string plate)
        {
            // Validamos que no exista la misma placa (ignorando mayúsculas/minúsculas)
            return await _context.Set<Vehicle>()
                .AnyAsync(v => v.Plate.ToLower() == plate.ToLower());
        }

        public async Task<bool> VehicleHasActiveProcessesAsync(int vehicleId)
        {
            // Verificamos si el vehículo tiene citas programadas que aún no concluyen
            return await _dbSet.AnyAsync(a =>
                a.VehicleId == vehicleId &&
                (a.Status == AppointmentStatus.Pending || a.Status == AppointmentStatus.Completed));
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(int vehicleId)
        {
            return await _context.Set<Vehicle>()
                .FirstOrDefaultAsync(v => v.Id == vehicleId);
        }

        public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
        {
            await _context.Set<Vehicle>().AddAsync(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        public async Task UpdateVehicleAsync(Vehicle vehicle)
        {
            _context.Set<Vehicle>().Update(vehicle);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> OwnerExistsAsync(int ownerId)
        {
            return await _context.Set<Owner>()
                .AnyAsync(o => o.Id == ownerId);
        }

        public async Task<IEnumerable<MaintenanceHistory>> GetHistoryByVehicleIdAsync(int vehicleId)
        {
            return await _context.Set<MaintenanceHistory>()
                .Where(mh => mh.VehicleId == vehicleId)
                .OrderByDescending(mh => mh.Date) 
                .ToListAsync();
        }
    }
}