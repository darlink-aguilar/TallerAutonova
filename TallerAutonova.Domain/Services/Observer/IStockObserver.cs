using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.Services.Observer
{
    public interface IStockObserver
    {
        void actualizar(Part repuesto);
    }
}