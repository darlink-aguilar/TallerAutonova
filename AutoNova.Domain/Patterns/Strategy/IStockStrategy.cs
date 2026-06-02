namespace AutoNova.Domain.Patterns.Strategy;

public interface IStockStrategy
{
    int Execute(int currentQuantity, int amount);
}
