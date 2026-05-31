namespace TallerAutonova.Domain.Entities
{
    public class Receptionist : User
    {
        // Navigation Property
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<MaintenanceHistory> MaintenanceHistory { get; set; } = new List<MaintenanceHistory>();
    }
}
