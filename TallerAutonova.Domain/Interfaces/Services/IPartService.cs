using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.Interfaces.Services
{
    public interface IPartService
    {
        // Part CRUD
        Task<Part?> GetByIdAsync(int id);
        Task<IEnumerable<Part>> GetAllAsync();
        Task<Part> CreateAsync(Part part);
        Task UpdateAsync(Part part);
        Task DeleteAsync(int id);

        // Stock management (Observer)
        Task<bool> ReduceStockAsync(int partId, int quantity);
        Task<bool> IncreaseStockAsync(int partId, int quantity);
        Task<int> GetCurrentStockAsync(int partId);
        Task<IEnumerable<Part>> GetLowStockPartsAsync();
        Task<IEnumerable<Part>> GetPartsByAdministratorAsync(int administratorId);
    }
}