using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.States;

public class CancelledState : IAppointmentState
{
    public void Start(Appointment appointment)
    {
        throw new InvalidOperationException(
            "Cancelled appointment cannot start.");
    }

    public void Complete(Appointment appointment)
    {
        throw new InvalidOperationException(
            "Cancelled appointment cannot complete.");
    }

    public void Cancel(Appointment appointment)
    {
        throw new InvalidOperationException(
            "Appointment already cancelled.");
    }
}