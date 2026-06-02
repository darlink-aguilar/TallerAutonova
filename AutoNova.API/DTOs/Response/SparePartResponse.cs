namespace AutoNova.API.DTOs.Response;

public class SparePartResponse
{
    public Guid     Id           { get; set; }
    public string   Code         { get; set; } = string.Empty;
    public string   Name         { get; set; } = string.Empty;
    public int      Quantity     { get; set; }
    public int      MinimumStock { get; set; }
    public bool     IsLowStock   { get; set; }
    public DateTime CreatedAt    { get; set; }
}
