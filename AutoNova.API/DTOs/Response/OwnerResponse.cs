namespace AutoNova.API.DTOs.Response;

public class OwnerResponse
{
    public Guid   Id             { get; set; }
    public string FullName       { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string Email          { get; set; } = string.Empty;
    public string Phone          { get; set; } = string.Empty;
    public string Address        { get; set; } = string.Empty;
    public Guid   VehicleId      { get; set; }
}
