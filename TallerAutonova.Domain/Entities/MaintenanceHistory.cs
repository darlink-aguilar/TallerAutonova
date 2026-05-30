namespace TallerAutonova.Domain.Entities
{
    public class MaintenanceHistory
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } 
        public string Observaciones { get; set; } = string.Empty; 

        // Navigation Property

    }
}