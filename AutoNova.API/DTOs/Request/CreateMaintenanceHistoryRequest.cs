using System.ComponentModel.DataAnnotations;

namespace AutoNova.API.DTOs.Request;

public class CreateMaintenanceHistoryRequest
{
    [Required(ErrorMessage = "El vehículo es obligatorio.")]
    public Guid VehicleId { get; set; }

    [MaxLength(1000)]
    public string Observation { get; set; } = string.Empty;

    [Required(ErrorMessage = "El servicio realizado es obligatorio.")]
    [MaxLength(500)]
    public string ServicePerformed { get; set; } = string.Empty;

    [Required(ErrorMessage = "El mecánico es obligatorio.")]
    public Guid MechanicId { get; set; }
}
