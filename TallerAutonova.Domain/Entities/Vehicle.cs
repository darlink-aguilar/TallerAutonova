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

        // Navigation Property
        public Owner Owner { get; set; } = null!;
        public ICollection<MaintenanceHistory> MaintenanceHistories { get; set; } = new List<MaintenanceHistory>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    }
}