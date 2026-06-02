namespace AutoNova.Domain.Patterns.Strategy;

public class WithdrawStockStrategy : IStockStrategy
{
    public int Execute(int currentQuantity, int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("La cantidad a retirar debe ser mayor a cero.");

        if (amount > currentQuantity)
            throw new InvalidOperationException(
                $"No se puede retirar {amount} unidades. Stock disponible: {currentQuantity}.");

        return currentQuantity - amount;
    }
}
