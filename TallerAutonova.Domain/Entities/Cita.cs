using TallerAutonova.Domain.Enums;

namespace TallerAutonova.Domain.Entities
{
    public class Cita
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } // En el diagrama esta como Time
        public TimeOnly Hora { get; set; } // En el diagrama esta como Time
        public string Descripcion { get; set; } = string.Empty; 
        public State Estado { get; set; }

        // Navigation Property

    }
}