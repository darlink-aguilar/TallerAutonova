using AutoNova.Domain.Entities;
using AutoNova.Domain.Interfaces.Repositories;
using AutoNova.Domain.Interfaces.Services;
using AutoNova.Domain.Patterns.Observer;
using AutoNova.Domain.Patterns.Strategy;

namespace AutoNova.Domain.Services;

public class SparePartService : ISparePartService
{
    private readonly ISparePartRepository _repository;
    private readonly IStockStrategy       _addStrategy      = new AddStockStrategy();
    private readonly IStockStrategy       _withdrawStrategy = new WithdrawStockStrategy();

    public SparePartService(ISparePartRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SparePart>> GetAllAsync() =>
        await _repository.GetAllAsync();

    public async Task<SparePart> GetByIdAsync(Guid id)
    {
        var part = await _repository.GetByIdAsync(id);
        if (part is null)
            throw new KeyNotFoundException($"Repuesto con ID {id} no encontrado.");
        return part;
    }

    public async Task<IEnumerable<SparePart>> GetLowStockAsync() =>
        await _repository.GetLowStockAsync();

    public async Task<(SparePart Part, IReadOnlyList<string> Alerts)> CreateAsync(
        string code, string name, int quantity, int minimumStock)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad al registrar no puede ser cero ni negativa.");

        if (await _repository.ExistsByCodeAsync(code))
            throw new InvalidOperationException($"Ya existe un repuesto con el código '{code}'.");

        if (await _repository.ExistsByNameAsync(name))
            throw new InvalidOperationException($"Ya existe un repuesto con el nombre '{name}'.");

        var part = new SparePart
        {
            Id           = Guid.NewGuid(),
            Code         = code.ToUpper(),
            Name         = name,
            Quantity     = quantity,
            MinimumStock = minimumStock,
            CreatedAt    = DateTime.UtcNow
        };

        await _repository.AddAsync(part);

        var alerts = CheckAlert(part);
        return (part, alerts);
    }

    public async Task<(SparePart Part, IReadOnlyList<string> Alerts)> AddStockAsync(Guid id, int amount)
    {
        var part      = await GetByIdAsync(id);
        part.Quantity = _addStrategy.Execute(part.Quantity, amount);

        await _repository.UpdateAsync(part);

        var alerts = CheckAlert(part);
        return (part, alerts);
    }

    public async Task<(SparePart Part, IReadOnlyList<string> Alerts)> WithdrawStockAsync(Guid id, int amount)
    {
        var part      = await GetByIdAsync(id);
        part.Quantity = _withdrawStrategy.Execute(part.Quantity, amount);

        await _repository.UpdateAsync(part);

        var alerts = CheckAlert(part);
        return (part, alerts);
    }

    public async Task<IReadOnlyList<string>> GetAlertsAsync()
    {
        var observer = new StockAlertObserver();
        var parts    = await _repository.GetAllAsync();

        foreach (var part in parts)
            observer.Update(part);

        return observer.Alerts.AsReadOnly();
    }

    private static IReadOnlyList<string> CheckAlert(SparePart part)
    {
        var observer = new StockAlertObserver();
        observer.Update(part);
        return observer.Alerts.AsReadOnly();
    }
}
