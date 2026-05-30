using TallerAutonova.Domain.Enums;

namespace TallerAutonova.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } 
        // public TimeOnly Hora { get; set; } 
        public string Descripcion { get; set; } = string.Empty; 
        public AppointmentStatus Estado { get; set; }

        // Navigation Property

    }
}