namespace TallerAutonova.Domain.Entities
{
    public class MaintenanceHistory
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } 
        public string Observaciones { get; set; } = string.Empty;
        public int VehicleId { get; set; };
        public int MechanicId { get; set; }

        // Navigation Property

        public Vehicle Vehicle { get; set; } = null!;
        public Mechanic Mechanic { get; set; } = null!;

    }
}