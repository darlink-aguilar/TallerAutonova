using AutoNova.DataAccess.Context;
using AutoNova.Domain.Entities;
using AutoNova.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AutoNova.DataAccess.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ApplicationDbContext _context;

    public AppointmentRepository(ApplicationDbContext context) => _context = context;

    public async Task<Appointment?> GetByIdAsync(Guid id) =>
        await _context.Appointments
            .Include(a => a.Vehicle).ThenInclude(v => v.Owner)
            .Include(a => a.Mechanic)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<IEnumerable<Appointment>> GetAllAsync() =>
        await _context.Appointments
            .Include(a => a.Vehicle).ThenInclude(v => v.Owner)
            .Include(a => a.Mechanic)
            .OrderByDescending(a => a.Date).ThenBy(a => a.Time)
            .ToListAsync();

    public async Task<IEnumerable<Appointment>> GetByVehicleIdAsync(Guid vehicleId) =>
        await _context.Appointments
            .Include(a => a.Vehicle).ThenInclude(v => v.Owner)
            .Include(a => a.Mechanic)
            .Where(a => a.VehicleId == vehicleId)
            .OrderByDescending(a => a.Date).ThenBy(a => a.Time)
            .ToListAsync();

    public async Task<IEnumerable<Appointment>> GetByMechanicIdAsync(Guid mechanicId) =>
        await _context.Appointments
            .Include(a => a.Vehicle).ThenInclude(v => v.Owner)
            .Include(a => a.Mechanic)
            .Where(a => a.MechanicId == mechanicId)
            .OrderByDescending(a => a.Date).ThenBy(a => a.Time)
            .ToListAsync();

    public async Task<bool> ExistsByDateAndTimeAsync(DateTime date, TimeSpan time, Guid? excludeId = null)
    {
        var query = _context.Appointments
            .Where(a => a.Date.Date == date.Date && a.Time == time);

        if (excludeId.HasValue)
            query = query.Where(a => a.Id != excludeId.Value);

        return await query.AnyAsync();
    }

    public async Task AddAsync(Appointment appointment)
    {
        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();
    }
}
