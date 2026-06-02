namespace AutoNova.Domain.Patterns.Strategy;

public class AddStockStrategy : IStockStrategy
{
    public int Execute(int currentQuantity, int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("La cantidad a agregar debe ser mayor a cero.");

        return currentQuantity + amount;
    }
}
