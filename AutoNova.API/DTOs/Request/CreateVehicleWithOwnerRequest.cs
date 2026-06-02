using System.ComponentModel.DataAnnotations;

namespace AutoNova.API.DTOs.Request;

public class CreateVehicleWithOwnerRequest
{
    // Vehicle
    [Required(ErrorMessage = "La placa es obligatoria.")]
    [MaxLength(20)]
    public string Plate { get; set; } = string.Empty;

    [Required(ErrorMessage = "La marca es obligatoria.")]
    [MaxLength(100)]
    public string Brand { get; set; } = string.Empty;

    [Required(ErrorMessage = "El modelo es obligatorio.")]
    [MaxLength(100)]
    public string Model { get; set; } = string.Empty;

    [Required(ErrorMessage = "El año es obligatorio.")]
    [Range(1901, 2100, ErrorMessage = "El año debe ser mayor a 1900.")]
    public int Year { get; set; }

    [MaxLength(50)]
    public string Color { get; set; } = string.Empty;

    // Owner
    [Required(ErrorMessage = "El nombre del propietario es obligatorio.")]
    [MaxLength(200)]
    public string OwnerFullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El número de documento es obligatorio.")]
    [MaxLength(50)]
    public string OwnerDocumentNumber { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "El correo del propietario no tiene un formato válido.")]
    [MaxLength(200)]
    public string OwnerEmail { get; set; } = string.Empty;

    [MaxLength(20)]
    public string OwnerPhone { get; set; } = string.Empty;

    [MaxLength(300)]
    public string OwnerAddress { get; set; } = string.Empty;
}
