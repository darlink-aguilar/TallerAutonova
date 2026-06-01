using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.Interfaces.Services
{
    public interface IMaintenanceHistoryService
    {
        // MaintenanceHistory CRUD
        Task<MaintenanceHistory?> GetByIdAsync(int id);
        Task<IEnumerable<MaintenanceHistory>> GetAllAsync();
        Task<MaintenanceHistory> CreateAsync(MaintenanceHistory history);
        Task UpdateAsync(MaintenanceHistory history);
        Task DeleteAsync(int id);

        // Specific queries
        Task<IEnumerable<MaintenanceHistory>> GetByVehicleIdAsync(int vehicleId);
        Task<IEnumerable<MaintenanceHistory>> GetByMechanicIdAsync(int mechanicId);
        Task<IEnumerable<MaintenanceHistory>> GetByReceptionistIdAsync(int receptionistId);
        Task<IEnumerable<MaintenanceHistory>> GetByDateRangeAsync(DateTime start, DateTime end);
    }
}