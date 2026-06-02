using AutoNova.Domain.Entities;

namespace AutoNova.Domain.Patterns.Observer;

public interface IStockSubject
{
    void RegisterObserver(IStockObserver observer);
    void RemoveObserver(IStockObserver observer);
    void NotifyObservers(SparePart sparePart);
}
