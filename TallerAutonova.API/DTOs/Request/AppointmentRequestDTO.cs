namespace TallerAutonova.API.DTOs.Request
{
    public class AppointmentRequestDTO
    {
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string? Description { get; set; } = string.Empty;
        public int VehicleId { get; set; }
        public int MechanicId { get; set; }
    }
}
