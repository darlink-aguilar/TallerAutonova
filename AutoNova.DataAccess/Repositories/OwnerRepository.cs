using AutoNova.DataAccess.Context;
using AutoNova.Domain.Entities;
using AutoNova.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AutoNova.DataAccess.Repositories;

public class OwnerRepository : IOwnerRepository
{
    private readonly ApplicationDbContext _context;

    public OwnerRepository(ApplicationDbContext context) => _context = context;

    public async Task<Owner?> GetByIdAsync(Guid id) =>
        await _context.Owners
            .Include(o => o.Vehicle)
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<Owner?> GetByVehicleIdAsync(Guid vehicleId) =>
        await _context.Owners
            .Include(o => o.Vehicle)
            .FirstOrDefaultAsync(o => o.VehicleId == vehicleId);

    public async Task<IEnumerable<Owner>> GetAllAsync() =>
        await _context.Owners
            .Include(o => o.Vehicle)
            .OrderBy(o => o.FullName)
            .ToListAsync();

    public async Task AddAsync(Owner owner)
    {
        await _context.Owners.AddAsync(owner);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Owner owner)
    {
        _context.Owners.Update(owner);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByDocumentNumberAsync(string documentNumber, Guid? excludeId = null)
    {
        var query = _context.Owners
            .Where(o => o.DocumentNumber.ToLower() == documentNumber.ToLower());

        if (excludeId.HasValue)
            query = query.Where(o => o.Id != excludeId.Value);

        return await query.AnyAsync();
    }
}
