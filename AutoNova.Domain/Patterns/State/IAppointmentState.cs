using AutoNova.Domain.Entities;
using AutoNova.Domain.Enums;

namespace AutoNova.Domain.Patterns.State;

public interface IAppointmentState
{
    void Start(Appointment appointment);
    void Complete(Appointment appointment);
    void Cancel(Appointment appointment);
    AppointmentStatus GetStatus();
}
