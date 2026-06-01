using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Enums;

namespace TallerAutonova.Domain.Interfaces.Services
{
    public interface IAdministratorService
    {
        // Administrator CRUD
        Task<Administrator?> GetByIdAsync(int id);
        Task<IEnumerable<Administrator>> GetAllAsync();
        Task<Administrator> CreateAsync(Administrator administrator);
        Task UpdateAsync(Administrator administrator);
        Task DeleteAsync(int id);

        // User management (Factory Method)
        Task<User> CreateUserAsync(string login, string password, string name, int administratorId, UserRole role);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<IEnumerable<Mechanic>> GetAllMechanicsAsync();
        Task<IEnumerable<Receptionist>> GetAllReceptionistsAsync();
        Task DeleteUserAsync(int id);
    }
}