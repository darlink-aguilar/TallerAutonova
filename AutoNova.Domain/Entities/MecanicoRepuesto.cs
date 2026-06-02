namespace AutoNova.Domain.Entities;

/// <summary>Tabla intermedia Mecánico ↔ Repuesto (retiro de stock por mecánico).</summary>
public class MecanicoRepuesto
{
    public Guid Id          { get; set; }
    public Guid MecanicoId  { get; set; }
    public Guid SparePartId { get; set; }
    public int  Quantity    { get; set; }
    public DateTime Date    { get; set; }

    public Mecanico   Mecanico   { get; set; } = null!;
    public SparePart  SparePart  { get; set; } = null!;
}
