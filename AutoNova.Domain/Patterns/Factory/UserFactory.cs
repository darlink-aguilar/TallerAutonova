using AutoNova.Domain.Entities;
using AutoNova.Domain.Enums;

namespace AutoNova.Domain.Patterns.Factory;

/// <summary>
/// Factory Method: el Administrador solo puede crear Mecánicos y Recepcionistas.
/// La creación de otro Administrador queda prohibida por diseño.
/// </summary>
public static class UserFactory
{
    public static User Create(string name, string email, string passwordHash, UserRole role)
    {
        return role switch
        {
            UserRole.Mecanico      => CreateMecanico(name, email, passwordHash),
            UserRole.Recepcionista => CreateRecepcionista(name, email, passwordHash),
            UserRole.Administrador => throw new InvalidOperationException(
                "Un Administrador no puede ser creado mediante el Factory Method. " +
                "Solo se pueden crear Mecánicos y Recepcionistas."),
            _ => throw new ArgumentException($"Rol no reconocido: {role}")
        };
    }

    public static Mecanico CreateMecanico(string name, string email, string passwordHash) =>
        new()
        {
            Id           = Guid.NewGuid(),
            Name         = name,
            Email        = email,
            PasswordHash = passwordHash,
            Role         = UserRole.Mecanico,
            IsActive     = true,
            CreatedAt    = DateTime.UtcNow
        };

    public static Recepcionista CreateRecepcionista(string name, string email, string passwordHash) =>
        new()
        {
            Id           = Guid.NewGuid(),
            Name         = name,
            Email        = email,
            PasswordHash = passwordHash,
            Role         = UserRole.Recepcionista,
            IsActive     = true,
            CreatedAt    = DateTime.UtcNow
        };
}
