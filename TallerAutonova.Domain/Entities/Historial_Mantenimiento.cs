namespace TallerAutonova.Domain.Entities
{
    public class Historial_Mantenimiento
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } // En el diagrama esta como Time
        public string Observaciones { get; set; } = string.Empty; 

        // Navigation Property

    }
}