using System.ComponentModel.DataAnnotations;

namespace AutoNova.API.DTOs.Request;

public class UpdateOwnerRequest
{
    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El número de documento es obligatorio.")]
    [MaxLength(50)]
    public string DocumentNumber { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(300)]
    public string Address { get; set; } = string.Empty;
}
