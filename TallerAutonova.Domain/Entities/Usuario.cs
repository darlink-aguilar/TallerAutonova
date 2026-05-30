using TallerAutonova.Domain.Enums;

namespace TallerAutonova.Domain.Entities
{
    public class Usuario 
    {
        public int Id { get; set; } 
        public string Nombre { get; set; } = string.Empty;
        public string Usuari { get; set; } = string.Empty; // Nombre de usuario 
        public string Contraseña { get; set; } = string.Empty;
        public Role Rol { get; set; }

        // Navigation Property
        
    }
}