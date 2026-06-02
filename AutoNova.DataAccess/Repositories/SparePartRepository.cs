using AutoNova.DataAccess.Context;
using AutoNova.Domain.Entities;
using AutoNova.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AutoNova.DataAccess.Repositories;

public class SparePartRepository : ISparePartRepository
{
    private readonly ApplicationDbContext _context;

    public SparePartRepository(ApplicationDbContext context) => _context = context;

    public async Task<SparePart?> GetByIdAsync(Guid id) =>
        await _context.SpareParts.FindAsync(id);

    public async Task<IEnumerable<SparePart>> GetAllAsync() =>
        await _context.SpareParts.OrderBy(s => s.Name).ToListAsync();

    public async Task<IEnumerable<SparePart>> GetLowStockAsync(int threshold = 8) =>
        await _context.SpareParts
            .Where(s => s.Quantity < threshold)
            .OrderBy(s => s.Quantity)
            .ToListAsync();

    public async Task AddAsync(SparePart sparePart)
    {
        await _context.SpareParts.AddAsync(sparePart);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SparePart sparePart)
    {
        _context.SpareParts.Update(sparePart);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByCodeAsync(string code, Guid? excludeId = null)
    {
        var query = _context.SpareParts
            .Where(s => s.Code.ToUpper() == code.ToUpper());

        if (excludeId.HasValue)
            query = query.Where(s => s.Id != excludeId.Value);

        return await query.AnyAsync();
    }

    public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null)
    {
        var query = _context.SpareParts
            .Where(s => s.Name.ToLower() == name.ToLower());

        if (excludeId.HasValue)
            query = query.Where(s => s.Id != excludeId.Value);

        return await query.AnyAsync();
    }
}
