using TallerAutonova.Domain.Enums;

namespace TallerAutonova.Domain.Entities
{
    public class User 
    {
        public int Id { get; set; } 
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty; // Nombre de usuario 
        public string Contraseña { get; set; } = string.Empty;
        public UserRole Rol { get; set; }
        // public int AdministratorId { get; set; } // Para relacionar con el administrador que creó el usuario

        // Navigation Property
        // public Administrator Administrator { get; set; } = null!;

    }
}