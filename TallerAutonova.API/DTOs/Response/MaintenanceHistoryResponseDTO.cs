namespace TallerAutonova.API.DTOs.Response
{
    public class MaintenanceHistoryResponseDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Observations { get; set; } = string.Empty;
        public int VehicleId { get; set; }
        public int MechanicId { get; set; }
        public string? MechanicName { get; set; }
        public string? VehiclePlate { get; set; }
        // public string? ReceptionistName { get; set; } 
    }
}