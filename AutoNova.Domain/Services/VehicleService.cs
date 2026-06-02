using AutoNova.Domain.Entities;
using AutoNova.Domain.Interfaces.Repositories;
using AutoNova.Domain.Interfaces.Services;

namespace AutoNova.Domain.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IOwnerRepository   _ownerRepository;

    public VehicleService(IVehicleRepository vehicleRepository, IOwnerRepository ownerRepository)
    {
        _vehicleRepository = vehicleRepository;
        _ownerRepository   = ownerRepository;
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync() =>
        await _vehicleRepository.GetAllAsync();

    public async Task<Vehicle> GetByIdAsync(Guid id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);
        if (vehicle is null)
            throw new KeyNotFoundException($"Vehículo con ID {id} no encontrado.");
        return vehicle;
    }

    public async Task<Vehicle> GetByPlateAsync(string plate)
    {
        var vehicle = await _vehicleRepository.GetByPlateAsync(plate.ToUpper());
        if (vehicle is null)
            throw new KeyNotFoundException($"Vehículo con placa '{plate}' no encontrado.");
        return vehicle;
    }

    public async Task<Vehicle> CreateAsync(Vehicle vehicle, Owner owner)
    {
        if (await _vehicleRepository.ExistsByPlateAsync(vehicle.Plate.ToUpper()))
            throw new InvalidOperationException($"Ya existe un vehículo con la placa '{vehicle.Plate}'.");

        if (await _ownerRepository.ExistsByDocumentNumberAsync(owner.DocumentNumber))
            throw new InvalidOperationException($"Ya existe un propietario con el documento '{owner.DocumentNumber}'.");

        ValidateYear(vehicle.Year);

        vehicle.Id        = Guid.NewGuid();
        vehicle.Plate     = vehicle.Plate.ToUpper();
        vehicle.IsActive  = true;
        vehicle.CreatedAt = DateTime.UtcNow;

        owner.Id        = Guid.NewGuid();
        owner.VehicleId = vehicle.Id;
        vehicle.Owner   = owner;

        await _vehicleRepository.AddAsync(vehicle);
        return vehicle;
    }

    public async Task<Vehicle> UpdateAsync(Guid id, string plate, string brand, string model, int year, string color)
    {
        var vehicle = await GetByIdAsync(id);

        if (!vehicle.Plate.Equals(plate.ToUpper(), StringComparison.OrdinalIgnoreCase) &&
            await _vehicleRepository.ExistsByPlateAsync(plate.ToUpper()))
            throw new InvalidOperationException($"La placa '{plate}' ya está registrada en otro vehículo.");

        ValidateYear(year);

        vehicle.Plate = plate.ToUpper();
        vehicle.Brand = brand;
        vehicle.Model = model;
        vehicle.Year  = year;
        vehicle.Color = color;

        await _vehicleRepository.UpdateAsync(vehicle);
        return vehicle;
    }

    public async Task DeactivateAsync(Guid id)
    {
        var vehicle     = await GetByIdAsync(id);
        vehicle.IsActive = false;
        await _vehicleRepository.UpdateAsync(vehicle);
    }

    public async Task ActivateAsync(Guid id)
    {
        var vehicle     = await GetByIdAsync(id);
        vehicle.IsActive = true;
        await _vehicleRepository.UpdateAsync(vehicle);
    }

    private static void ValidateYear(int year)
    {
        if (year <= 1900)
            throw new ArgumentException("El año del vehículo debe ser mayor a 1900.");
        if (year > DateTime.UtcNow.Year)
            throw new ArgumentException($"El año del vehículo no puede ser mayor al año actual ({DateTime.UtcNow.Year}).");
    }
}
