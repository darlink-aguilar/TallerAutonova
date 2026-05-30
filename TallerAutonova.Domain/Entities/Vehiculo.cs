namespace TallerAutonova.Domain.Entities
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty; 
        public string Modelo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public int Año { get; set; } 

        // Navigation Property

    }
}