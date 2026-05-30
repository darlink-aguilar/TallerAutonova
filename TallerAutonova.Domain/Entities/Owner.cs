namespace TallerAutonova.Domain.Entities
{
    public class Owner
    {
        public int Id { get; set; }  
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        // Navigation Property

        public Vehicle Vehicle { get; set; } = null!;

    }
}