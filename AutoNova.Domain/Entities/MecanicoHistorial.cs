namespace AutoNova.Domain.Entities;

/// <summary>Tabla intermedia Mecánico ↔ HistorialMantenimiento (M:N).</summary>
public class MecanicoHistorial
{
    public Guid MecanicoId          { get; set; }
    public Guid MaintenanceHistoryId { get; set; }
    public DateTime Date            { get; set; }

    public Mecanico           Mecanico           { get; set; } = null!;
    public MaintenanceHistory MaintenanceHistory { get; set; } = null!;
}
