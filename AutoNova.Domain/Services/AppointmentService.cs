using AutoNova.Domain.Entities;
using AutoNova.Domain.Interfaces.Repositories;
using AutoNova.Domain.Interfaces.Services;

namespace AutoNova.Domain.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IVehicleRepository     _vehicleRepository;

    public AppointmentService(IAppointmentRepository appointmentRepository, IVehicleRepository vehicleRepository)
    {
        _appointmentRepository = appointmentRepository;
        _vehicleRepository     = vehicleRepository;
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync() =>
        await _appointmentRepository.GetAllAsync();

    public async Task<Appointment> GetByIdAsync(Guid id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);
        if (appointment is null)
            throw new KeyNotFoundException($"Cita con ID {id} no encontrada.");
        return appointment;
    }

    public async Task<IEnumerable<Appointment>> GetByVehicleIdAsync(Guid vehicleId) =>
        await _appointmentRepository.GetByVehicleIdAsync(vehicleId);

    public async Task<IEnumerable<Appointment>> GetByMechanicIdAsync(Guid mechanicId) =>
        await _appointmentRepository.GetByMechanicIdAsync(mechanicId);

    public async Task<Appointment> CreateAsync(DateTime date, TimeSpan time, string description,
        Guid vehicleId, Guid? mechanicId)
    {
        if (await _vehicleRepository.GetByIdAsync(vehicleId) is null)
            throw new KeyNotFoundException($"Vehículo con ID {vehicleId} no encontrado.");

        if (await _appointmentRepository.ExistsByDateAndTimeAsync(date, time))
            throw new InvalidOperationException(
                $"Ya existe una cita agendada para el {date:dd/MM/yyyy} a las {time:hh\\:mm}.");

        var appointment = new Appointment
        {
            Id          = Guid.NewGuid(),
            Date        = date.Date,
            Time        = time,
            Description = description,
            VehicleId   = vehicleId,
            MechanicId  = mechanicId,
            CreatedAt   = DateTime.UtcNow
        };

        await _appointmentRepository.AddAsync(appointment);
        return appointment;
    }

    public async Task<Appointment> UpdateAsync(Guid id, DateTime date, TimeSpan time, string description,
        Guid vehicleId, Guid? mechanicId)
    {
        var appointment = await GetByIdAsync(id);

        if (await _appointmentRepository.ExistsByDateAndTimeAsync(date, time, id))
            throw new InvalidOperationException(
                $"Ya existe una cita agendada para el {date:dd/MM/yyyy} a las {time:hh\\:mm}.");

        appointment.Date        = date.Date;
        appointment.Time        = time;
        appointment.Description = description;
        appointment.VehicleId   = vehicleId;
        appointment.MechanicId  = mechanicId;

        await _appointmentRepository.UpdateAsync(appointment);
        return appointment;
    }

    public async Task StartAsync(Guid id)
    {
        var appointment = await GetByIdAsync(id);
        appointment.Start();
        await _appointmentRepository.UpdateAsync(appointment);
    }

    public async Task CompleteAsync(Guid id)
    {
        var appointment = await GetByIdAsync(id);
        appointment.Complete();
        await _appointmentRepository.UpdateAsync(appointment);
    }

    public async Task CancelAsync(Guid id)
    {
        var appointment = await GetByIdAsync(id);
        appointment.Cancel();
        await _appointmentRepository.UpdateAsync(appointment);
    }
}
