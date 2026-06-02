using Microsoft.Extensions.Logging;
using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Interfaces.Repositories;
using TallerAutonova.Domain.Interfaces.Services;
using TallerAutonova.Domain.States;

namespace TallerAutonova.Domain.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehiclesRepository _vehicleRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly ILogger<VehicleService> _logger;

    public VehicleService(
    IVehiclesRepository vehicleRepository,
    IOwnerRepository ownerRepository,
    ILogger<VehicleService> logger)
    {
        _vehicleRepository = vehicleRepository;
        _ownerRepository = ownerRepository;
        _logger = logger;
    }

    public async Task<Vehicle?> GetByIdAsync(int id)
    {
        _logger.LogInformation(
        "Retrieving Vehicle with ID: {VehicleId}",
        id);

        var Vehicle =
            await _vehicleRepository
                .GetByIdWithOwnerAsync(id);

        if (Vehicle == null)
        {
            _logger.LogWarning(
                "Vehicle with ID {VehicleId} not found", id);
        }

        return Vehicle;
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all Vehicles");
        return await _vehicleRepository.GetAllWithOwnerAsync();
    }

    public async Task<IEnumerable<Vehicle>> GetAllByOwnerAsync(string owner)
    {
        _logger.LogInformation("Retrieving all Vehicles for Owner: {Owner}", owner);
        return await _vehicleRepository.GetByOwnerNameAsync(owner);
    }

    public async Task<IEnumerable<Vehicle>> GetAllByBrandAsync(string brand)
    {
        _logger.LogInformation("Retrieving all Vehicles for Brand: {Brand}", brand);
        return await _vehicleRepository.GetByBrandIdAsync(brand);
    }

    public async Task<Vehicle?> CreateAsync(Vehicle Vehicle, string ownerName, string ownerPhone)
    {
        if (await _vehicleRepository.ExistsWithPlateAsync(Vehicle.Plate))
        {
            throw new InvalidOperationException(
                "A vehicle with the same plate already exists.");
        }

        var owner = new Owner
        {
            Name = ownerName,
            Phone = ownerPhone
        };

        owner =
            await _ownerRepository
                .CreateAsync(owner);

        _logger.LogInformation(
        "Creating Vehicle with Plate: {Plate}",
        Vehicle.Plate);

        return await _vehicleRepository.CreateAsync(Vehicle);
    }

    public async Task UpdateAsync(int id, Vehicle Vehicle)
    {
        var existingVehicle = await _vehicleRepository.GetByIdAsync(id);

        if (existingVehicle == null)
        {
            throw new KeyNotFoundException(
            $"No se encontró la cita con ID {id}");
        }

        existingVehicle.State = Vehicle.State;

        _logger.LogInformation("Updating Vehicle with ID: {VehicleId}", id);
        await _vehicleRepository.UpdateAsync(existingVehicle);
    }

    public async Task UpdateOwnerAsync(int id, Owner owner)
    {
        var existingOwner = await _ownerRepository.GetByIdAsync(id);

        if (existingOwner == null)
        {
            throw new KeyNotFoundException(
            $"No se encontró la cita con ID {id}");
        }

        existingOwner.Name = owner.Name;
        existingOwner.Phone = owner.Phone;

        _logger.LogInformation("Updating Owner with ID: {OwnerId}", id);
        await _ownerRepository.UpdateAsync(existingOwner);
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _vehicleRepository.ExistsAsync(id);
        if (!exists)
        {
            throw new KeyNotFoundException(
            $"No se encontró la cita con ID {id}");
        }

        _logger.LogInformation("Deleting Vehicle with ID: {VehicleId}", id);
        await _vehicleRepository.DeleteAsync(id);
    }
}