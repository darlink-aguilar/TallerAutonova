using System.ComponentModel.DataAnnotations;

namespace AutoNova.API.DTOs.Request;

public class ChangeAppointmentStatusRequest
{
    /// <summary>Valores permitidos: "start" | "complete" | "cancel"</summary>
    [Required(ErrorMessage = "La acción es obligatoria.")]
    public string Action { get; set; } = string.Empty;
}
