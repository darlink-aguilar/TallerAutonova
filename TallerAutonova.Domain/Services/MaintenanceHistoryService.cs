using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Interfaces.Repositories;
using TallerAutonova.Domain.Interfaces.Services;

namespace TallerAutonova.Domain.Services
{
    public class MaintenanceHistoryService : IMaintenanceHistoryService
    {
        private readonly IMaintenanceHistoryRepository _historyRepository;
        private readonly IGenericRepository<Vehicle> _vehicleRepository;
        private readonly IGenericRepository<Mechanic> _mechanicRepository;
        private readonly IGenericRepository<Receptionist> _receptionistRepository;

        public MaintenanceHistoryService(
            IMaintenanceHistoryRepository historyRepository,
            IGenericRepository<Vehicle> vehicleRepository,
            IGenericRepository<Mechanic> mechanicRepository,
            IGenericRepository<Receptionist> receptionistRepository)
        {
            _historyRepository = historyRepository;
            _vehicleRepository = vehicleRepository;
            _mechanicRepository = mechanicRepository;
            _receptionistRepository = receptionistRepository;
        }

        // MaintenanceHistory CRUD
        public async Task<MaintenanceHistory?> GetByIdAsync(int id)
            => await _historyRepository.GetByIdAsync(id);

        public async Task<IEnumerable<MaintenanceHistory>> GetAllAsync()
            => await _historyRepository.GetAllAsync();

        public async Task<MaintenanceHistory> CreateAsync(MaintenanceHistory history)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(history.VehicleId);
            if (vehicle == null)
                throw new Exception($"Vehículo con ID {history.VehicleId} no encontrado");

            var mechanic = await _mechanicRepository.GetByIdAsync(history.MechanicId);
            if (mechanic == null)
                throw new Exception($"Mecánico con ID {history.MechanicId} no encontrado");

            var receptionist = await _receptionistRepository.GetByIdAsync(history.ReceptionistId);
            if (receptionist == null)
                throw new Exception($"Recepcionista con ID {history.ReceptionistId} no encontrado");

            history.Date = DateTime.UtcNow;
            return await _historyRepository.CreateAsync(history);
        }

        public async Task UpdateAsync(MaintenanceHistory history)
            => await _historyRepository.UpdateAsync(history);

        public async Task DeleteAsync(int id)
            => await _historyRepository.DeleteAsync(id);

        // Specific queries
        public async Task<IEnumerable<MaintenanceHistory>> GetByVehicleIdAsync(int vehicleId)
            => await _historyRepository.GetByVehicleIdAsync(vehicleId);

        public async Task<IEnumerable<MaintenanceHistory>> GetByMechanicIdAsync(int mechanicId)
            => await _historyRepository.GetByMechanicIdAsync(mechanicId);

        public async Task<IEnumerable<MaintenanceHistory>> GetByReceptionistIdAsync(int receptionistId)
            => await _historyRepository.GetByReceptionistIdAsync(receptionistId);

        public async Task<IEnumerable<MaintenanceHistory>> GetByDateRangeAsync(DateTime start, DateTime end)
            => await _historyRepository.GetByDateRangeAsync(start, end);
    }
}