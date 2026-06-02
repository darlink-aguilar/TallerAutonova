using System.ComponentModel.DataAnnotations;

namespace AutoNova.API.DTOs.Request;

public class UpdateStockRequest
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
    public int Amount { get; set; }
}
