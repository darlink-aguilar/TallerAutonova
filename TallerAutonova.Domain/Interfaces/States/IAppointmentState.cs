using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.States;

public interface IAppointmentState
{
    void Start(Appointment appointment);

    void Complete(Appointment appointment);

    void Cancel(Appointment appointment);
}