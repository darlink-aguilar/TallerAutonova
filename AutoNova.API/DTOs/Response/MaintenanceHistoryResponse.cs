namespace AutoNova.API.DTOs.Response;

public class MaintenanceHistoryResponse
{
    public Guid     Id               { get; set; }
    public Guid     VehicleId        { get; set; }
    public string   Plate            { get; set; } = string.Empty;
    public string   Brand            { get; set; } = string.Empty;
    public string   Model            { get; set; } = string.Empty;
    public string   OwnerName        { get; set; } = string.Empty;
    public string   Observation      { get; set; } = string.Empty;
    public string   ServicePerformed { get; set; } = string.Empty;
    public DateTime CreatedAt        { get; set; }
    public Guid     MechanicId       { get; set; }
    public string   MechanicName     { get; set; } = string.Empty;
}
