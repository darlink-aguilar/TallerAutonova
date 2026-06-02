using AutoNova.DataAccess.Context;
using AutoNova.Domain.Entities;
using AutoNova.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AutoNova.DataAccess.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly ApplicationDbContext _context;

    public VehicleRepository(ApplicationDbContext context) => _context = context;

    public async Task<Vehicle?> GetByIdAsync(Guid id) =>
        await _context.Vehicles
            .Include(v => v.Owner)
            .FirstOrDefaultAsync(v => v.Id == id);

    public async Task<Vehicle?> GetByPlateAsync(string plate) =>
        await _context.Vehicles
            .Include(v => v.Owner)
            .FirstOrDefaultAsync(v => v.Plate.ToUpper() == plate.ToUpper());

    public async Task<IEnumerable<Vehicle>> GetAllAsync() =>
        await _context.Vehicles
            .Include(v => v.Owner)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync();

    public async Task AddAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vehicle vehicle)
    {
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByPlateAsync(string plate) =>
        await _context.Vehicles
            .AnyAsync(v => v.Plate.ToUpper() == plate.ToUpper());
}
