using AutoNova.Domain.Entities;
using AutoNova.Domain.Enums;

namespace AutoNova.Domain.Patterns.State;

public class PendingState : IAppointmentState
{
    public void Start(Appointment appointment) =>
        appointment.SetState(new InProgressState());

    public void Complete(Appointment appointment) =>
        throw new InvalidOperationException("No se puede completar una cita que aún está pendiente.");

    public void Cancel(Appointment appointment) =>
        appointment.SetState(new CancelledState());

    public AppointmentStatus GetStatus() => AppointmentStatus.Pendiente;
}
