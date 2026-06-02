using System.ComponentModel.DataAnnotations;

namespace AutoNova.API.DTOs.Request;

public class UpdateAppointmentRequest
{
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "La hora es obligatoria.")]
    public TimeSpan Time { get; set; }

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "El vehículo es obligatorio.")]
    public Guid VehicleId { get; set; }

    public Guid? MechanicId { get; set; }
}
