using AutoNova.Domain.Entities;
using AutoNova.Domain.Interfaces.Repositories;
using AutoNova.Domain.Interfaces.Services;

namespace AutoNova.Domain.Services;

public class MaintenanceHistoryService : IMaintenanceHistoryService
{
    private readonly IMaintenanceHistoryRepository _historyRepository;
    private readonly IVehicleRepository            _vehicleRepository;

    public MaintenanceHistoryService(
        IMaintenanceHistoryRepository historyRepository,
        IVehicleRepository vehicleRepository)
    {
        _historyRepository = historyRepository;
        _vehicleRepository = vehicleRepository;
    }

    public async Task<IEnumerable<MaintenanceHistory>> GetAllAsync() =>
        await _historyRepository.GetAllAsync();

    public async Task<MaintenanceHistory> GetByIdAsync(Guid id)
    {
        var history = await _historyRepository.GetByIdAsync(id);
        if (history is null)
            throw new KeyNotFoundException($"Historial de mantenimiento con ID {id} no encontrado.");
        return history;
    }

    public async Task<IEnumerable<MaintenanceHistory>> GetByVehicleIdAsync(Guid vehicleId) =>
        await _historyRepository.GetByVehicleIdAsync(vehicleId);

    public async Task<IEnumerable<MaintenanceHistory>> GetByPlateAsync(string plate) =>
        await _historyRepository.GetByPlateAsync(plate.ToUpper());

    public async Task<MaintenanceHistory> AddAsync(Guid vehicleId, string observation,
        string servicePerformed, Guid mechanicId)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
        if (vehicle is null)
            throw new KeyNotFoundException($"Vehículo con ID {vehicleId} no encontrado.");

        var history = new MaintenanceHistory
        {
            Id               = Guid.NewGuid(),
            VehicleId        = vehicleId,
            Observation      = observation,
            ServicePerformed = servicePerformed,
            MechanicId       = mechanicId,
            CreatedAt        = DateTime.UtcNow
        };

        await _historyRepository.AddAsync(history);
        return history;
    }
}
