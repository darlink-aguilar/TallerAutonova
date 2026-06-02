using AutoNova.Domain.Enums;
using AutoNova.Domain.Patterns.State;

namespace AutoNova.Domain.Entities;

public class Appointment
{
    private IAppointmentState? _state;

    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public string Description { get; set; } = string.Empty;
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pendiente;
    public Guid VehicleId { get; set; }
    public Guid? MechanicId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Vehicle Vehicle { get; set; } = null!;
    public User? Mechanic { get; set; }

    private IAppointmentState CurrentState => _state ??= Status switch
    {
        AppointmentStatus.Pendiente  => new PendingState(),
        AppointmentStatus.EnProceso  => new InProgressState(),
        AppointmentStatus.Completada => new CompletedState(),
        AppointmentStatus.Cancelada  => new CancelledState(),
        _                            => new PendingState()
    };

    public void SetState(IAppointmentState state)
    {
        _state = state;
        Status = _state.GetStatus();
    }

    public void Start()    => CurrentState.Start(this);
    public void Complete() => CurrentState.Complete(this);
    public void Cancel()   => CurrentState.Cancel(this);
}
