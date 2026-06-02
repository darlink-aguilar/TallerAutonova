using AutoNova.Domain.Entities;
using AutoNova.Domain.Enums;

namespace AutoNova.Domain.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetByIdAsync(Guid id);
    Task<User> CreateAsync(string name, string email, string password, UserRole role);
    Task<User> UpdateAsync(Guid id, string name, string email, UserRole role);
    Task DeactivateAsync(Guid id);
    Task ActivateAsync(Guid id);
    Task ChangePasswordAsync(Guid id, string newPassword);
}
