using AutoNova.Domain.Entities;
using AutoNova.Domain.Enums;

namespace AutoNova.Domain.Patterns.State;

public class CancelledState : IAppointmentState
{
    public void Start(Appointment appointment) =>
        throw new InvalidOperationException("La cita está cancelada y no puede iniciarse.");

    public void Complete(Appointment appointment) =>
        throw new InvalidOperationException("La cita está cancelada y no puede completarse.");

    public void Cancel(Appointment appointment) =>
        throw new InvalidOperationException("La cita ya está cancelada.");

    public AppointmentStatus GetStatus() => AppointmentStatus.Cancelada;
}
