using TallerAutonova.Domain.Enums;

namespace TallerAutonova.API.DTOs.Response
{
    public class AppointmentResponseDTO
    {
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string Description { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public int VehicleId { get; set; }
        public int MechanicId { get; set; }
    }
}
