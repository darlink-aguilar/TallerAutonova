namespace TallerAutonova.Domain.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty; 
        public string Modelo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public bool Estado { get; set; } = true; // Deberia ser booleano 
        public int Año { get; set; }

        // Navigation Property

        public Owner owner { get; set; } = null!;
        public ICollection<MaintenanceHistory> MaintenanceHistories { get; set; } = new List<MaintenanceHistory>;

    }
}