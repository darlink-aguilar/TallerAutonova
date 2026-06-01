namespace TallerAutonova.API.DTOs.Request
{
    public class MaintenanceHistoryRequestDTO // PREGUNTAR VALERIA
    {
        public int VehicleId { get; set; }
        public int MechanicId { get; set; }
        // public int ReceptionistId { get; set; } El recepcionista también puede añadir observaciones
        public string Observations { get; set; } = string.Empty;
    }
}