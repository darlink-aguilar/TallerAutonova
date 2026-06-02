using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Enums;
using TallerAutonova.Domain.Interfaces.Repositories;
using TallerAutonova.Domain.Interfaces.Services;
using TallerAutonova.Domain.Services.FactoryMethod;

namespace TallerAutonova.Domain.Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly IAdministratorRepository _adminRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly UsuarioFactoryConcrete _usuarioFactory;

        public AdministratorService(
            IAdministratorRepository adminRepository,
            IGenericRepository<User> userRepository,
            UsuarioFactoryConcrete usuarioFactory)
        {
            _adminRepository = adminRepository;
            _userRepository = userRepository;
            _usuarioFactory = usuarioFactory;
        }

        // Administrator CRUD
        public async Task<Administrator?> GetByIdAsync(int id)
            => await _adminRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Administrator>> GetAllAsync()
            => await _adminRepository.GetAllAsync();

        public async Task<Administrator> CreateAsync(Administrator administrator)
            => await _adminRepository.CreateAsync(administrator);

        public async Task UpdateAsync(Administrator administrator)
            => await _adminRepository.UpdateAsync(administrator);

        public async Task DeleteAsync(int id)
            => await _adminRepository.DeleteAsync(id);

        // User management (Factory Method)
        public async Task<User> CreateUserAsync(string login, string password, string name, int administratorId, UserRole role)
        {
            var admin = await _adminRepository.GetByIdAsync(administratorId);
            if (admin == null)
                throw new Exception($"Administrador con ID {administratorId} no encontrado");

            var nuevoUsuario = _usuarioFactory.registerNewEmployee(login, password, name, administratorId, role);
            return await _userRepository.CreateAsync(nuevoUsuario);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
            => await _userRepository.GetAllAsync();

        public async Task<User?> GetUserByIdAsync(int id)
            => await _userRepository.GetByIdAsync(id);

        // SOLUCIÓN: Traer todos los usuarios y filtrar en memoria (LINQ to Objects)
        public async Task<IEnumerable<Mechanic>> GetAllMechanicsAsync()
        {
            var allUsers = await _userRepository.GetAllAsync();
            return allUsers.Where(u => u.Role == UserRole.Mechanic).Cast<Mechanic>().ToList();
        }

        // SOLUCIÓN: Traer todos los usuarios y filtrar en memoria (LINQ to Objects)
        public async Task<IEnumerable<Receptionist>> GetAllReceptionistsAsync()
        {
            var allUsers = await _userRepository.GetAllAsync();
            return allUsers.Where(u => u.Role == UserRole.Receptionist).Cast<Receptionist>().ToList();
        }

        public async Task DeleteUserAsync(int id)
            => await _userRepository.DeleteAsync(id);
    }
}