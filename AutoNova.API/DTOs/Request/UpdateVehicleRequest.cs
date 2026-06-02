using System.ComponentModel.DataAnnotations;

namespace AutoNova.API.DTOs.Request;

public class UpdateVehicleRequest
{
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
}
