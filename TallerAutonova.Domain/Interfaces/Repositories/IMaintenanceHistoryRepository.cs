using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.Interfaces.Repositories
{
    public interface IMaintenanceHistoryRepository : IGenericRepository<MaintenanceHistory>
    {
        // Specific queries
        Task<IEnumerable<MaintenanceHistory>> GetByVehicleIdAsync(int vehicleId);
        Task<IEnumerable<MaintenanceHistory>> GetByMechanicIdAsync(int mechanicId);
        Task<IEnumerable<MaintenanceHistory>> GetByReceptionistIdAsync(int receptionistId);
        Task<IEnumerable<MaintenanceHistory>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<MaintenanceHistory>> GetRecentAsync(int count);
    }
}