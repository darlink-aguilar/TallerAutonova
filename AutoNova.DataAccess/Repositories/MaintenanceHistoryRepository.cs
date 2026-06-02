using AutoNova.DataAccess.Context;
using AutoNova.Domain.Entities;
using AutoNova.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AutoNova.DataAccess.Repositories;

public class MaintenanceHistoryRepository : IMaintenanceHistoryRepository
{
    private readonly ApplicationDbContext _context;

    public MaintenanceHistoryRepository(ApplicationDbContext context) => _context = context;

    public async Task<MaintenanceHistory?> GetByIdAsync(Guid id) =>
        await _context.MaintenanceHistories
            .Include(m => m.Vehicle).ThenInclude(v => v.Owner)
            .Include(m => m.Mechanic)
            .FirstOrDefaultAsync(m => m.Id == id);

    public async Task<IEnumerable<MaintenanceHistory>> GetAllAsync() =>
        await _context.MaintenanceHistories
            .Include(m => m.Vehicle).ThenInclude(v => v.Owner)
            .Include(m => m.Mechanic)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<MaintenanceHistory>> GetByVehicleIdAsync(Guid vehicleId) =>
        await _context.MaintenanceHistories
            .Include(m => m.Vehicle).ThenInclude(v => v.Owner)
            .Include(m => m.Mechanic)
            .Where(m => m.VehicleId == vehicleId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<MaintenanceHistory>> GetByPlateAsync(string plate) =>
        await _context.MaintenanceHistories
            .Include(m => m.Vehicle).ThenInclude(v => v.Owner)
            .Include(m => m.Mechanic)
            .Where(m => m.Vehicle.Plate.ToUpper() == plate.ToUpper())
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

    public async Task AddAsync(MaintenanceHistory history)
    {
        await _context.MaintenanceHistories.AddAsync(history);
        await _context.SaveChangesAsync();
    }
}
