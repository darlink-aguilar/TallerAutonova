using TallerAutonova.Domain.Enums;

namespace TallerAutonova.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateOnly Fecha { get; set; } 
        public TimeOnly Hora { get; set; } 
        public string Descripcion { get; set; } = string.Empty; 
        public AppointmentStatus Estado { get; set; } = AppointmentStatus.Pending;
        public int VehicleId { get; set; }

        // Navigation Property

        public Vehicle Vehicle { get; set; } = null!;

    }
}