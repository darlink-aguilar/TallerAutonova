namespace TallerAutonova.Domain.Entities
{
    public class Propoetario
    {
        public int Id { get; set; } // Falta Id en el diagrama 
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty; 
        // Navigation Property

    }
}