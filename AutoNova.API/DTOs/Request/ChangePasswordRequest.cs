using System.ComponentModel.DataAnnotations;

namespace AutoNova.API.DTOs.Request;

public class ChangePasswordRequest
{
    [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string NewPassword { get; set; } = string.Empty;
}
