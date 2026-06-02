namespace AutoNova.Domain.Entities;

public class MaintenanceHistory
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public string Observation { get; set; } = string.Empty;
    public string ServicePerformed { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid MechanicId { get; set; }

    public Vehicle Vehicle { get; set; } = null!;
    public User Mechanic { get; set; } = null!;
}
