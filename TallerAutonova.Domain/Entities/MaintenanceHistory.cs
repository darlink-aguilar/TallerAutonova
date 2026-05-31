namespace TallerAutonova.Domain.Entities
{
    public class MaintenanceHistory
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } 
        public string Observations { get; set; } = string.Empty;
        public int VehicleId { get; set; } // FK
        public int MechanicId { get; set; } // FK
        public int ReceptionistId { get; set; } // FK

        // Navigation Property
        public Vehicle Vehicle { get; set; } = null!;
        public Mechanic Mechanic { get; set; } = null!;
        public Receptionist Receptionist { get; set; } = null!;

    }
}