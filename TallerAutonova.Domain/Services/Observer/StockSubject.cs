using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Services.Observer;

namespace TallerAutonova.Services.Observer
{
    public class StockSubject
    {
        private readonly List<IStockObserver> _observers = new();

        public void Attach(IStockObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void Detach(IStockObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(Part repuesto)
        {
            if (repuesto.Quantity <= repuesto.MinStock)
            {
                foreach (var observer in _observers)
                {
                    observer.actualizar(repuesto);
                }
            }
        }

        public int GetObserverCount()
        {
            return _observers.Count;
        }
    }
}