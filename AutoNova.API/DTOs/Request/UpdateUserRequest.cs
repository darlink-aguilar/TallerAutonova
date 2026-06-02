using System.ComponentModel.DataAnnotations;
using AutoNova.Domain.Enums;

namespace AutoNova.API.DTOs.Request;

public class UpdateUserRequest
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El rol es obligatorio.")]
    public UserRole Role { get; set; }
}
