namespace TallerAutonova.Domain.Entities
{
    public class Administrador
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Usuari { get; set; } = string.Empty; // Nombre de usuario 
        public string Contraseña { get; set; } = string.Empty;

        // Navigation Property

    }
}