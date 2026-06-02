namespace AutoNova.API.DTOs.Response;

public class StockOperationResponse
{
    public SparePartResponse      Part      { get; set; } = null!;
    public IReadOnlyList<string>  Alerts    { get; set; } = Array.Empty<string>();
    public bool                   HasAlerts => Alerts.Count > 0;
}
