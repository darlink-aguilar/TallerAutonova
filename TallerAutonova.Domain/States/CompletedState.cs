using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.States;

public class CompletedState : IAppointmentState
{
    public void Start(Appointment appointment)
    {
        throw new InvalidOperationException(
            "Completed appointment cannot restart.");
    }

    public void Complete(Appointment appointment)
    {
        throw new InvalidOperationException(
            "Appointment already completed.");
    }

    public void Cancel(Appointment appointment)
    {
        throw new InvalidOperationException(
            "Completed appointment cannot be cancelled.");
    }
}