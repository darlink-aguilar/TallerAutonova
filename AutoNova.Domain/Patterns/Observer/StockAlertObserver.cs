using AutoNova.Domain.Entities;

namespace AutoNova.Domain.Patterns.Observer;

public class StockAlertObserver : IStockObserver
{
    private const int LowStockThreshold = 8;
    public List<string> Alerts { get; } = new();

    public void Update(SparePart sparePart)
    {
        if (sparePart.Quantity < LowStockThreshold)
        {
            Alerts.Add(
                $"ALERTA DE STOCK BAJO: Repuesto '{sparePart.Name}' " +
                $"(Código: {sparePart.Code}) — Stock actual: {sparePart.Quantity} unidades " +
                $"(mínimo recomendado: {LowStockThreshold}).");
        }
    }
}
