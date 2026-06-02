using Microsoft.Extensions.Logging;
using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Interfaces.Repositories;
using TallerAutonova.Domain.Interfaces.Services;
using TallerAutonova.Domain.States;

namespace TallerAutonova.Domain.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ILogger<AppointmentService> _logger;

    public AppointmentService(
    IAppointmentRepository appointmentRepository,
    ILogger<AppointmentService> logger)
    {
        _appointmentRepository = appointmentRepository;
        _logger = logger;
    }

    public async Task StartAsync(int id)
    {
        var appointment =
            await _appointmentRepository.GetByIdAsync(id);

        var state =
            AppointmentStateFactory
                .GetState(appointment!.State);

        state.Start(appointment);

        await _appointmentRepository.UpdateAsync(
            appointment);
    }

    public async Task CompleteAsync(int id)
    {
        var appointment =
            await _appointmentRepository.GetByIdAsync(id);

        var state =
            AppointmentStateFactory
                .GetState(appointment!.State);

        state.Complete(appointment);

        await _appointmentRepository.UpdateAsync(
            appointment);
    }

    public async Task CancelAsync(int id)
    {
        var appointment =
            await _appointmentRepository.GetByIdAsync(id);

        var state =
            AppointmentStateFactory
                .GetState(appointment!.State);

        state.Cancel(appointment);

        await _appointmentRepository.UpdateAsync(
            appointment);
    }

    public async Task<Appointment?> GetByIdAsync(int id)
    {
        _logger.LogInformation(
        "Retrieving Appointment with ID: {AppointmentId}",
        id);

        var appointment =
            await _appointmentRepository
                .GetByIdWithVehicleAsync(id);

        if (appointment == null)
        {
            _logger.LogWarning(
                "Appointment with ID {AppointmentId} not found",id);
        }

        return appointment;
    }

    public async Task<Appointment?> GetByMechanicIdAsync(int mechanicId)
    {
        _logger.LogInformation(
        "Retrieving Appointment with ID: {AppointmentId}",
        mechanicId);

        var appointment =
            await _appointmentRepository
                .GetByMechanicIdAsync(mechanicId);

        if (appointment == null)
        {
            _logger.LogWarning(
                "Appointment with ID {AppointmentId} not found",mechanicId);
        }

        return appointment;
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all Appointments");
        return await _appointmentRepository.GetAllWithDetailsAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAllByMechanicIdAsync(int mechanicId)
    {
        _logger.LogInformation("Retrieving all Appointments for Mechanic ID: {MechanicId}", mechanicId);
        return await _appointmentRepository.GetAllByMechanicIdAsync(mechanicId);
    }

    public async Task<IEnumerable<Appointment>> GetAllByVehiclePlateAsync(string vehiclePlate)
    {
        _logger.LogInformation("Retrieving all Appointments for Vehicle Plate: {VehiclePlate}", vehiclePlate);
        return await _appointmentRepository.GetAllByVehiclePlateAsync(vehiclePlate);
    }

    public async Task<Appointment> CreateAsync(Appointment Appointment)
    {
        // Validar que el vehiculo existe
        var vehicleExists = await _appointmentRepository.ExistsAsync(Appointment.VehicleId);
        if (!vehicleExists)
        {
            _logger.LogWarning("Vehicle with ID {VehicleId} not found", Appointment.VehicleId);
            throw new KeyNotFoundException(
            $"No se encontró el vehículo con ID {Appointment.VehicleId}");
        }

        // Validar que el mecánico existe
        var mechanicExists = await _appointmentRepository.ExistsAsync(Appointment.MechanicId);
        if (!mechanicExists)
        {
            _logger.LogWarning("Mechanic with ID {MechanicId} not found", Appointment.MechanicId);
            throw new KeyNotFoundException(
            $"No se encontró el mecánico con ID {Appointment.MechanicId}");
        }

        // Validar que la fecha no sea en el pasado
        if (Appointment.Date < DateOnly.FromDateTime(DateTime.Now))
        {
            throw new InvalidOperationException(
                "Cannot schedule appointments in past dates.");
        }

        // Validar que el mecánico no tenga otra cita en la misma fecha y hora
        var existsConflict =
            await _appointmentRepository
                .ExistsConflictAsync(
                    Appointment.MechanicId,
                    Appointment.Date,
                    Appointment.Time);

        if (existsConflict)
        {
            throw new InvalidOperationException(
                "The mechanic already has an appointment at that date and time.");
        }

        _logger.LogInformation(
        "Creating Appointment for {Date} at {Time}",
        Appointment.Date, Appointment.Time);

        return await _appointmentRepository.CreateAsync(Appointment);
    }

    public async Task UpdateAsync(int id, Appointment Appointment)
    {
        var existingAppointment = await _appointmentRepository.GetByIdAsync(id);

        if (existingAppointment == null)
        {
            throw new KeyNotFoundException(
            $"No se encontró la cita con ID {id}");
        }

        existingAppointment.Description = Appointment.Description;

        _logger.LogInformation("Updating Appointment with ID: {AppointmentId}", id);
        await _appointmentRepository.UpdateAsync(existingAppointment);
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _appointmentRepository.ExistsAsync(id);
        if (!exists)
        {
            throw new KeyNotFoundException(
            $"No se encontró la cita con ID {id}");
        }

        _logger.LogInformation("Deleting Appointment with ID: {AppointmentId}", id);
        await _appointmentRepository.DeleteAsync(id);
    }
}