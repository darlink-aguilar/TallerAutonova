using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Interfaces.Repositories;
using TallerAutonova.Domain.Interfaces.Services;
using TallerAutonova.Services.Observer;

namespace TallerAutonova.Domain.Services
{
    public class PartService : IPartService
    {
        private readonly IPartRepository _partRepository;
        private readonly IAdministratorRepository _adminRepository;
        private readonly StockSubject _stockSubject;

        public PartService(
            IPartRepository partRepository,
            IAdministratorRepository adminRepository,
            StockSubject stockSubject)
        {
            _partRepository = partRepository;
            _adminRepository = adminRepository;
            _stockSubject = stockSubject;
        }

        public async Task<Part?> GetByIdAsync(int id)
            => await _partRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Part>> GetAllAsync()
            => await _partRepository.GetAllAsync();

        public async Task<Part> CreateAsync(Part part)
        {
            var admin = await _adminRepository.GetByIdAsync(part.AdministratorId);
            if (admin == null)
                throw new Exception($"Administrador con ID {part.AdministratorId} no encontrado");

            return await _partRepository.CreateAsync(part);
        }

        public async Task UpdateAsync(Part part)
            => await _partRepository.UpdateAsync(part);

        public async Task DeleteAsync(int id)
            => await _partRepository.DeleteAsync(id);

        public async Task<bool> ReduceStockAsync(int partId, int quantity)
        {
            // Obtener el repuesto
            var repuesto = await _partRepository.GetByIdAsync(partId);

            // Si no existe, retornar false
            if (repuesto == null)
                return false;

            // Si no hay stock suficiente, retornar false
            if (repuesto.Quantity < quantity)
                return false;

            // Reducir stock
            repuesto.Quantity -= quantity;
            await _partRepository.UpdateAsync(repuesto);

            // Notificar al Observer si el stock está por debajo del mínimo
            if (repuesto.Quantity <= repuesto.MinStock)
            {
                _stockSubject.Notify(repuesto);
            }

            return true;
        }

        public async Task<bool> IncreaseStockAsync(int partId, int quantity)
        {
            var repuesto = await _partRepository.GetByIdAsync(partId);
            if (repuesto == null)
                return false;

            repuesto.Quantity += quantity;
            await _partRepository.UpdateAsync(repuesto);

            return true;
        }

        public async Task<int> GetCurrentStockAsync(int partId)
        {
            var repuesto = await _partRepository.GetByIdAsync(partId);
            return repuesto?.Quantity ?? 0;
        }

        public async Task<IEnumerable<Part>> GetLowStockPartsAsync()
        {
            var allParts = await _partRepository.GetAllAsync();
            return allParts.Where(p => p.Quantity <= p.MinStock).ToList();
        }

        public async Task<IEnumerable<Part>> GetPartsByAdministratorAsync(int administratorId)
        {
            var allParts = await _partRepository.GetAllAsync();
            return allParts.Where(p => p.AdministratorId == administratorId).ToList();
        }
    }
}