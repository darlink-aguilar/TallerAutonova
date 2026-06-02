using AutoNova.Domain.Enums;

namespace AutoNova.Domain.Entities;

public class Recepcionista : User
{
    public Recepcionista() => Role = UserRole.Recepcionista;
}
