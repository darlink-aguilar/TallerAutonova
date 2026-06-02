using AutoNova.Domain.Entities;

namespace AutoNova.Domain.Interfaces.Repositories;

public interface ISparePartRepository
{
    Task<SparePart?> GetByIdAsync(Guid id);
    Task<IEnumerable<SparePart>> GetAllAsync();
    Task<IEnumerable<SparePart>> GetLowStockAsync(int threshold = 8);
    Task AddAsync(SparePart sparePart);
    Task UpdateAsync(SparePart sparePart);
    Task<bool> ExistsByCodeAsync(string code, Guid? excludeId = null);
    Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);
}
