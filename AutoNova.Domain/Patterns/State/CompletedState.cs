using AutoNova.Domain.Entities;
using AutoNova.Domain.Enums;

namespace AutoNova.Domain.Patterns.State;

public class CompletedState : IAppointmentState
{
    public void Start(Appointment appointment) =>
        throw new InvalidOperationException("La cita ya fue completada.");

    public void Complete(Appointment appointment) =>
        throw new InvalidOperationException("La cita ya fue completada.");

    public void Cancel(Appointment appointment) =>
        throw new InvalidOperationException("No se puede cancelar una cita que ya fue completada.");

    public AppointmentStatus GetStatus() => AppointmentStatus.Completada;
}
