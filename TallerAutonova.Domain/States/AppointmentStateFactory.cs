using TallerAutonova.Domain.Enums;

namespace TallerAutonova.Domain.States;

public static class AppointmentStateFactory
{
    public static IAppointmentState GetState(
        AppointmentStatus status)
    {
        return status switch
        {
            AppointmentStatus.Pending
                => new PendingState(),

            AppointmentStatus.InProgress
                => new InProgressState(),

            AppointmentStatus.Completed
                => new CompletedState(),

            AppointmentStatus.Cancelled
                => new CancelledState(),

            _ => throw new ArgumentException(
                    "Invalid state")
        };
    }
}