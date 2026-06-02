using System.ComponentModel.DataAnnotations;

namespace AutoNova.API.DTOs.Request;

public class CreateSparePartRequest
{
    [Required(ErrorMessage = "El código es obligatorio.")]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
    public int Quantity { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El stock mínimo debe ser mayor a cero.")]
    public int MinimumStock { get; set; } = 8;
}
