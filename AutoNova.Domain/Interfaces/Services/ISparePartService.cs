using AutoNova.Domain.Entities;

namespace AutoNova.Domain.Interfaces.Services;

public interface ISparePartService
{
    Task<IEnumerable<SparePart>> GetAllAsync();
    Task<SparePart> GetByIdAsync(Guid id);
    Task<IEnumerable<SparePart>> GetLowStockAsync();
    Task<(SparePart Part, IReadOnlyList<string> Alerts)> CreateAsync(string code, string name, int quantity, int minimumStock);
    Task<(SparePart Part, IReadOnlyList<string> Alerts)> AddStockAsync(Guid id, int amount);
    Task<(SparePart Part, IReadOnlyList<string> Alerts)> WithdrawStockAsync(Guid id, int amount);
    Task<IReadOnlyList<string>> GetAlertsAsync();
}
