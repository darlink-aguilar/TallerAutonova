using AutoNova.Domain.Entities;

namespace AutoNova.Domain.Patterns.Observer;

public interface IStockObserver
{
    void Update(SparePart sparePart);
}
