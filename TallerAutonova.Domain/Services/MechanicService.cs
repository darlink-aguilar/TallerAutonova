using Microsoft.Extensions.Logging;
using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Interfaces.Repositories;
using TallerAutonova.Domain.Interfaces.Services;

namespace TallerAutonova.Domain.Services
{
    public class MechanicService : IMechanicService
    {
        private readonly IMechanicRepository _mechanicRepository;
        private readonly ILogger<MechanicService> _logger;

        public MechanicService(IMechanicRepository mechanicRepository, ILogger<MechanicService> logger)
        {
            _mechanicRepository = mechanicRepository;
            _logger = logger;
        }

        public async Task<MaintenanceHistory> AddObservationAsync(MaintenanceHistory maintenanceHistory)
        {
            // VALIDACIONES
            // No se permite guardar observaciones vacías
            if (string.IsNullOrWhiteSpace(maintenanceHistory.Observations))
            {
                _logger.LogWarning("Intento de registrar una observación vacía o nula para el vehículo ID: {VehicleId}", maintenanceHistory.VehicleId);
                throw new ArgumentException("La observación no puede estar vacía.");
            }

            // Las observaciones deben ser de minimo 20 caracteres
            if (maintenanceHistory.Observations.Trim().Length < 20)
            {
                _logger.LogWarning("La observación proporcionada es demasiado corta ({Length} caracteres) para el vehículo ID: {VehicleId}",
                    maintenanceHistory.Observations.Length, maintenanceHistory.VehicleId);
                throw new ArgumentException("La observación debe incluir una descripción de mínimo 20 caracteres.");
            }

            // Vehiculo debe de existir
            var vehicleExists = await _mechanicRepository.VehicleExistsAsync(maintenanceHistory.VehicleId);
            if (!vehicleExists)
            {
                _logger.LogWarning("Intento de registrar observación en un vehículo inexistente. ID: {VehicleId}", maintenanceHistory.VehicleId);
                throw new KeyNotFoundException($"El vehículo con ID {maintenanceHistory.VehicleId} no existe en el sistema.");
            }

            // Cada registro debe guardar la fecha automáticamente.
            maintenanceHistory.Date = DateTime.UtcNow;

            _logger.LogInformation("Registrando exitosamente una nueva observación para el vehículo ID: {VehicleId} por el mecánico ID: {MechanicId}",
                maintenanceHistory.VehicleId, maintenanceHistory.MechanicId);

            return await _mechanicRepository.CreateAsync(maintenanceHistory);
        }

        public async Task<IEnumerable<MaintenanceHistory>> GetHistoryByVehicleIdAsync(int vehicleId)
        {
            _logger.LogInformation("Consultando el historial de mantenimiento para el vehículo ID: {VehicleId}", vehicleId);

            // Verifica que el vehiculo existe
            var vehicleExists = await _mechanicRepository.VehicleExistsAsync(vehicleId);
            if (!vehicleExists)
            {
                _logger.LogWarning("Consulta de historial fallida: El vehículo ID {VehicleId} no existe.", vehicleId);
                throw new KeyNotFoundException($"No se encontró el vehículo con ID {vehicleId}");
            }

            return await _mechanicRepository.GetHistoryByVehicleIdAsync(vehicleId);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByMechanicIdAsync(int mechanicId)
        {
            _logger.LogInformation("Consultando las citas programadas para el mecánico ID: {MechanicId}", mechanicId);

            return await _mechanicRepository.GetAppointmentsByMechanicIdAsync(mechanicId);
        }
    }
}