using TallerAutonova.Domain.Enums;

namespace TallerAutonova.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; } 
        public TimeOnly Time { get; set; } 
        public string Description { get; set; } = string.Empty; 
        public AppointmentStatus State { get; set; } = AppointmentStatus.Pending;
        public int VehicleId { get; set; } // FK
        public int MechanicId { get; set; } // FK

        // Navigation Property
        public Vehicle Vehicle { get; set; } = null!;
        public Mechanic Mechanic { get; set; } = null!;

    }
}