using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Enums;

namespace TallerAutonova.Domain.States;

public class PendingState : IAppointmentState
{
    public void Start(Appointment appointment)
    {
        appointment.State =
            AppointmentStatus.InProgress;
    }

    public void Complete(Appointment appointment)
    {
        throw new InvalidOperationException(
            "Cannot complete a pending appointment.");
    }

    public void Cancel(Appointment appointment)
    {
        appointment.State =
            AppointmentStatus.Cancelled;
    }
}