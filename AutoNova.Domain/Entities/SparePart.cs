namespace AutoNova.Domain.Entities;

public class SparePart
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int MinimumStock { get; set; } = 8;
    public DateTime CreatedAt { get; set; }
}
