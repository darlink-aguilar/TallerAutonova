using AutoNova.Domain.Entities;

namespace AutoNova.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<(string Token, User User)?> LoginAsync(string email, string password);
    string GenerateToken(User user);
}
