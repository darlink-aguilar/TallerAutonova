using AutoNova.Domain.Enums;

namespace AutoNova.Domain.Entities;

public class Mecanico : User
{
    public Mecanico() => Role = UserRole.Mecanico;

    public ICollection<MecanicoRepuesto>  MecanicoRepuestos  { get; set; } = new List<MecanicoRepuesto>();
    public ICollection<MecanicoHistorial> MecanicoHistoriales { get; set; } = new List<MecanicoHistorial>();
}
