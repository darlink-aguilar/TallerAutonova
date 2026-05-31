namespace TallerAutonova.Domain.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Plate { get; set; } = string.Empty; 
        public string Model { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool State { get; set; } = true; // Deberia ser booleano 
        public int Year { get; set; }
        public int OwnerId { get; set; } // FK
        public int MaintenanceHistoryId { get; set; } // FK (1:1)

        // Navigation Property
        public Owner Owner { get; set; } = null!;
        public MaintenanceHistory MaintenanceHistory { get; set; } = null!; // 1:1
        public Appointment Appointment { get; set; } = null!; // 1:1

    }
}