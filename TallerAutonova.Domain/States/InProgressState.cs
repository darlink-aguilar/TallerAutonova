using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Enums;

namespace TallerAutonova.Domain.States;

public class InProgressState : IAppointmentState
{
    public void Start(Appointment appointment)
    {
        throw new InvalidOperationException(
            "Appointment already started.");
    }

    public void Complete(Appointment appointment)
    {
        appointment.State =
            AppointmentStatus.Completed;
    }

    public void Cancel(Appointment appointment)
    {
        appointment.State =
            AppointmentStatus.Cancelled;
    }
}