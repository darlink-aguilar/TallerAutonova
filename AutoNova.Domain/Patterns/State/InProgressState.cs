using AutoNova.Domain.Entities;
using AutoNova.Domain.Enums;

namespace AutoNova.Domain.Patterns.State;

public class InProgressState : IAppointmentState
{
    public void Start(Appointment appointment) =>
        throw new InvalidOperationException("La cita ya se encuentra en proceso.");

    public void Complete(Appointment appointment) =>
        appointment.SetState(new CompletedState());

    public void Cancel(Appointment appointment) =>
        appointment.SetState(new CancelledState());

    public AppointmentStatus GetStatus() => AppointmentStatus.EnProceso;
}
