using AutoNova.Domain.Entities;

namespace AutoNova.Domain.Interfaces.Repositories;

public interface IOwnerRepository
{
    Task<Owner?> GetByIdAsync(Guid id);
    Task<Owner?> GetByVehicleIdAsync(Guid vehicleId);
    Task<IEnumerable<Owner>> GetAllAsync();
    Task AddAsync(Owner owner);
    Task UpdateAsync(Owner owner);
    Task<bool> ExistsByDocumentNumberAsync(string documentNumber, Guid? excludeId = null);
}
