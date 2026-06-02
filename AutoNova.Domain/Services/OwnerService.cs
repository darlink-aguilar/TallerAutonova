using AutoNova.Domain.Entities;
using AutoNova.Domain.Interfaces.Repositories;
using AutoNova.Domain.Interfaces.Services;

namespace AutoNova.Domain.Services;

public class OwnerService : IOwnerService
{
    private readonly IOwnerRepository _ownerRepository;

    public OwnerService(IOwnerRepository ownerRepository)
    {
        _ownerRepository = ownerRepository;
    }

    public async Task<IEnumerable<Owner>> GetAllAsync() =>
        await _ownerRepository.GetAllAsync();

    public async Task<Owner> GetByIdAsync(Guid id)
    {
        var owner = await _ownerRepository.GetByIdAsync(id);
        if (owner is null)
            throw new KeyNotFoundException($"Propietario con ID {id} no encontrado.");
        return owner;
    }

    public async Task<Owner> GetByVehicleIdAsync(Guid vehicleId)
    {
        var owner = await _ownerRepository.GetByVehicleIdAsync(vehicleId);
        if (owner is null)
            throw new KeyNotFoundException($"No se encontró propietario para el vehículo con ID {vehicleId}.");
        return owner;
    }

    public async Task<Owner> UpdateAsync(Guid id, string fullName, string documentNumber,
        string email, string phone, string address)
    {
        var owner = await GetByIdAsync(id);

        if (!owner.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase) &&
            await _ownerRepository.ExistsByDocumentNumberAsync(documentNumber, id))
            throw new InvalidOperationException($"El número de documento '{documentNumber}' ya está registrado.");

        owner.FullName       = fullName;
        owner.DocumentNumber = documentNumber;
        owner.Email          = email;
        owner.Phone          = phone;
        owner.Address        = address;

        await _ownerRepository.UpdateAsync(owner);
        return owner;
    }
}
