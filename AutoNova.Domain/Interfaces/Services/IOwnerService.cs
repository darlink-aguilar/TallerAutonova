using AutoNova.Domain.Entities;

namespace AutoNova.Domain.Interfaces.Services;

public interface IOwnerService
{
    Task<IEnumerable<Owner>> GetAllAsync();
    Task<Owner> GetByIdAsync(Guid id);
    Task<Owner> GetByVehicleIdAsync(Guid vehicleId);
    Task<Owner> UpdateAsync(Guid id, string fullName, string documentNumber, string email, string phone, string address);
}
