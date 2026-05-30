namespace TallerAutonova.Domain.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty; 
        public string Modelo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty; // Deberia ser booleano 
        public int Año { get; set; } 

        // Navigation Property

    }
}