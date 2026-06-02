using AutoNova.Domain.Enums;

namespace AutoNova.Domain.Entities;

public class Administrador : User
{
    public Administrador() => Role = UserRole.Administrador;
}
