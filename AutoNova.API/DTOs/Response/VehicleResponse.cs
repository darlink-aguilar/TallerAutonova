namespace AutoNova.API.DTOs.Response;

public class VehicleResponse
{
    public Guid          Id        { get; set; }
    public string        Plate     { get; set; } = string.Empty;
    public string        Brand     { get; set; } = string.Empty;
    public string        Model     { get; set; } = string.Empty;
    public int           Year      { get; set; }
    public string        Color     { get; set; } = string.Empty;
    public bool          IsActive  { get; set; }
    public DateTime      CreatedAt { get; set; }
    public OwnerResponse? Owner    { get; set; }
}
