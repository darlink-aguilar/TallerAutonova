namespace AutoNova.API.DTOs.Response;

public class AppointmentResponse
{
    public Guid         Id          { get; set; }
    public DateTime     Date        { get; set; }
    public TimeSpan     Time        { get; set; }
    public string       Description { get; set; } = string.Empty;
    public string       Status      { get; set; } = string.Empty;
    public DateTime     CreatedAt   { get; set; }
    public Guid         VehicleId   { get; set; }
    public string       Plate       { get; set; } = string.Empty;
    public string       VehicleBrand { get; set; } = string.Empty;
    public string       VehicleModel { get; set; } = string.Empty;
    public string       OwnerName   { get; set; } = string.Empty;
    public Guid?        MechanicId  { get; set; }
    public string?      MechanicName { get; set; }
}
