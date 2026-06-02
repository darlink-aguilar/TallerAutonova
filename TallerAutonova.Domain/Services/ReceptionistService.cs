using Microsoft.Extensions.Logging;
using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Enums;
using TallerAutonova.Domain.Interfaces.Repositories;
using TallerAutonova.Domain.Interfaces.Services;

namespace TallerAutonova.Domain.Services
{
    public class ReceptionistService : IReceptionistService
    {
        private readonly IReceptionistRepository _receptionistRepository;
        private readonly ILogger<ReceptionistService> _logger;

        public ReceptionistService(IReceptionistRepository receptionistRepository, ILogger<ReceptionistService> logger)
        {
            _receptionistRepository = receptionistRepository;
            _logger = logger;
        }


        public async Task<Appointment> ScheduleAppointmentAsync(Appointment appointment)
        {
            // La fecha de la cita no puede ser anterior a la fecha actual
            if (appointment.Date < DateOnly.FromDateTime(DateTime.Today))
            {
                _logger.LogWarning("Intento de agendar cita en una fecha pasada: {Date}", appointment.Date);
                throw new ArgumentException("La fecha de la cita no puede ser anterior a la fecha actual.");
            }

            // La cita debe estar asociada a un vehículo registrado
            var vehicleExists = await _receptionistRepository.VehicleExistsAsync(appointment.VehicleId);
            if (!vehicleExists)
            {
                _logger.LogWarning("Intento de agendar cita para vehículo inexistente ID: {VehicleId}", appointment.VehicleId);
                throw new KeyNotFoundException("El vehículo especificado no se encuentra registrado.");
            }

            // No se pueden agendar dos citas en el mismo horario para el mismo mecánico
            var isOccupied = await _receptionistRepository.IsMechanicOccupiedAsync(appointment.MechanicId, appointment.Date, appointment.Time);
            if (isOccupied)
            {
                _logger.LogWarning("El mecánico ID {MechanicId} ya tiene una cita el {Date} a las {Time}", appointment.MechanicId, appointment.Date, appointment.Time);
                throw new InvalidOperationException("El horario seleccionado no está disponible para este mecánico.");
            }

            appointment.Status = AppointmentStatus.Pending; 

            _logger.LogInformation("Agendando exitosamente cita para el vehículo ID: {VehicleId}", appointment.VehicleId);
            return await _receptionistRepository.CreateAsync(appointment);
        }

        public async Task RescheduleAppointmentAsync(int id, DateTime newDateTime)
        {
            var appointment = await _receptionistRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                _logger.LogWarning("Cita ID {Id} no encontrada para reagendar.", id);
                throw new KeyNotFoundException($"No se encontró la cita con ID {id}");
            }

            // Solo se pueden reagendar citas futuras
            if (appointment.Date < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new InvalidOperationException("No se pueden modificar citas que pertenecen a días pasados.");
            }

            // No se permite reagendar una cita ya atendida o cancelada
            if (appointment.Status == AppointmentStatus.Canceled || appointment.Status == AppointmentStatus.Completed)
            {
                throw new InvalidOperationException("No se permite reagendar una cita que ya ha sido atendida o cancelada.");
            }

            var newDate = DateOnly.FromDateTime(newDateTime);
            var newTime = TimeOnly.FromDateTime(newDateTime);

            // La nueva fecha y hora deben estar disponible
            var isOccupied = await _receptionistRepository.IsMechanicOccupiedAsync(appointment.MechanicId, newDate, newTime, appointment.Id);
            if (isOccupied)
            {
                throw new InvalidOperationException("El nuevo horario solicitado no está disponible.");
            }

            appointment.Date = newDate;
            appointment.Time = newTime;

            _logger.LogInformation("Reagendando cita ID {Id} para el {Date} {Time}", id, newDate, newTime);
            await _receptionistRepository.UpdateAsync(appointment);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByMechanicIdAsync(int mechanicId)
        {
            _logger.LogInformation("Consultando citas asignadas al mecánico ID: {MechanicId} desde el flujo de recepcionista.", mechanicId);

            return await _receptionistRepository.GetAppointmentsByMechanicIdAsync(mechanicId);
        }

        public async Task CancelAppointmentAsync(int id)
        {
            var appointment = await _receptionistRepository.GetByIdAsync(id);
            if (appointment == null)
                throw new KeyNotFoundException($"No se encontró la cita con ID {id}");

            // Solo se pueden cancelar citas futuras
            if (appointment.Date < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new InvalidOperationException("No se pueden cancelar citas de fechas pasadas.");
            }

            // Cita no válida si ya está atendida
            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new InvalidOperationException("No se puede cancelar una cita que ya fue completada y atendida.");
            }

            appointment.Status = AppointmentStatus.Canceled;

            _logger.LogInformation("Cancelando cita ID: {Id}. El horario ha sido liberado.", id);
            await _receptionistRepository.UpdateAsync(appointment);
        }

        public async Task AddAppointmentDescriptionAsync(int id, string description)
        {
            var appointment = await _receptionistRepository.GetByIdAsync(id);
            if (appointment == null)
                throw new KeyNotFoundException($"No se encontró la cita con ID {id}");

            // La descripción debe tener un máximo de 300 caracteres
            if (!string.IsNullOrEmpty(description) && description.Length > 300)
            {
                throw new ArgumentException("La descripción supera el límite máximo permitido de 300 caracteres.");
            }

            appointment.Description = description;

            _logger.LogInformation("Añadiendo descripción a la cita ID: {Id}", id);
            await _receptionistRepository.UpdateAsync(appointment);
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            _logger.LogInformation("Consultando el listado completo de citas.");
            return await _receptionistRepository.GetAllAppointmentsOrderedAsync();
        }

        public async Task<Vehicle> RegisterVehicleAsync(Vehicle vehicle)
        {
            // Todos los campos obligatorios deben ser completados
            if (string.IsNullOrWhiteSpace(vehicle.Plate) || string.IsNullOrWhiteSpace(vehicle.Model) || vehicle.OwnerId <= 0)
            {
                throw new ArgumentException("Los datos obligatorios del vehículo están incompletos.");
            }

            // No se permite registrar vehículos con placas duplicadas
            var plateExists = await _receptionistRepository.PlateExistsAsync(vehicle.Plate);
            if (plateExists)
            {
                throw new InvalidOperationException($"Ya existe un vehículo registrado con la placa '{vehicle.Plate}'.");
            }

            // Por defecto el vehículo ingresa como activo
            vehicle.State = true;

            _logger.LogInformation("Registrando nuevo vehículo con placa: {Plate}", vehicle.Plate);
            return await _receptionistRepository.CreateVehicleAsync(vehicle);
        }

        public async Task DeactivateVehicleAsync(int vehicleId)
        {
            var vehicle = await _receptionistRepository.GetVehicleByIdAsync(vehicleId);
            if (vehicle == null)
                throw new KeyNotFoundException($"No se encontró el vehículo con ID {vehicleId}");

            // No se puede desactivar un vehículo con citas activas o servicios en proceso
            var hasActiveProcesses = await _receptionistRepository.VehicleHasActiveProcessesAsync(vehicleId);
            if (hasActiveProcesses)
            {
                throw new InvalidOperationException("No se puede desactivar el vehículo porque posee citas activas o procesos pendientes.");
            }

            // La desactivación debe ser lógica
            vehicle.State = false; 

            _logger.LogInformation("Desactivación lógica realizada para el vehículo ID: {VehicleId}", vehicleId);
            await _receptionistRepository.UpdateVehicleAsync(vehicle);
        }

        public async Task UpdateVehicleDataAsync(int vehicleId, Vehicle vehicle)
        {
            // El vehículo debe existir previamente en el sistema
            var existingVehicle = await _receptionistRepository.GetVehicleByIdAsync(vehicleId);
            if (existingVehicle == null)
                throw new KeyNotFoundException($"No se encontró el vehículo con ID {vehicleId}");

            // Los datos del propietario deben ser válidos y completos
            var ownerExists = await _receptionistRepository.OwnerExistsAsync(vehicle.OwnerId);
            if (!ownerExists)
            {
                throw new KeyNotFoundException($"El propietario con ID {vehicle.OwnerId} no es válido o no existe.");
            }

            if (string.IsNullOrWhiteSpace(vehicle.Plate) || string.IsNullOrWhiteSpace(vehicle.Model))
            {
                throw new ArgumentException("Los datos de actualización del vehículo están incompletos.");
            }

            // Si cambió la placa, validamos que no colisione con otra existente
            if (existingVehicle.Plate.ToLower() != vehicle.Plate.ToLower())
            {
                var plateExists = await _receptionistRepository.PlateExistsAsync(vehicle.Plate);
                if (plateExists)
                    throw new InvalidOperationException($"La placa '{vehicle.Plate}' ya está asignada a otro vehículo.");
            }

            existingVehicle.Plate = vehicle.Plate;
            existingVehicle.Brand = vehicle.Brand;
            existingVehicle.Model = vehicle.Model;
            existingVehicle.Year = vehicle.Year;
            existingVehicle.OwnerId = vehicle.OwnerId;

            _logger.LogInformation("Actualizando datos del vehículo ID: {VehicleId}", vehicleId);
            await _receptionistRepository.UpdateVehicleAsync(existingVehicle);
        }

        public async Task<IEnumerable<MaintenanceHistory>> GetVehicleHistoryAsync(int vehicleId)
        {
            // El historial debe estar asociado a un vehículo previamente registrado.
            var vehicleExists = await _receptionistRepository.VehicleExistsAsync(vehicleId);
            if (!vehicleExists)
            {
                throw new KeyNotFoundException($"No se puede consultar el historial. El vehículo con ID {vehicleId} no existe.");
            }

            _logger.LogInformation("Consultando historial de mantenimiento para el vehículo ID: {VehicleId}", vehicleId);
            return await _receptionistRepository.GetHistoryByVehicleIdAsync(vehicleId);
        }
    }
}