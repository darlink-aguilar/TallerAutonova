using AutoNova.Domain.Entities;
using AutoNova.Domain.Enums;
using AutoNova.Domain.Interfaces.Repositories;
using AutoNova.Domain.Interfaces.Services;
using AutoNova.Domain.Patterns.Factory;

namespace AutoNova.Domain.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> GetAllAsync() =>
        await _userRepository.GetAllAsync();

    public async Task<User> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
            throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");
        return user;
    }

    public async Task<User> CreateAsync(string name, string email, string password, UserRole role)
    {
        if (await _userRepository.ExistsByEmailAsync(email))
            throw new InvalidOperationException($"Ya existe un usuario con el correo '{email}'.");

        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = UserFactory.Create(name, email, hash, role);

        await _userRepository.AddAsync(user);
        return user;
    }

    public async Task<User> UpdateAsync(Guid id, string name, string email, UserRole role)
    {
        var user = await GetByIdAsync(id);

        if (!user.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
            await _userRepository.ExistsByEmailAsync(email))
            throw new InvalidOperationException($"El correo '{email}' ya está en uso por otro usuario.");

        user.Name  = name;
        user.Email = email;
        user.Role  = role;

        await _userRepository.UpdateAsync(user);
        return user;
    }

    public async Task DeactivateAsync(Guid id)
    {
        var user    = await GetByIdAsync(id);
        user.IsActive = false;
        await _userRepository.UpdateAsync(user);
    }

    public async Task ActivateAsync(Guid id)
    {
        var user    = await GetByIdAsync(id);
        user.IsActive = true;
        await _userRepository.UpdateAsync(user);
    }

    public async Task ChangePasswordAsync(Guid id, string newPassword)
    {
        var user         = await GetByIdAsync(id);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _userRepository.UpdateAsync(user);
    }
}
