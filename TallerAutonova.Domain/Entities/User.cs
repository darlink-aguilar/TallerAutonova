using System.Numerics;
using TallerAutonova.Domain.Enums;

namespace TallerAutonova.Domain.Entities
{
    public class User 
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty; // Nombre de usuario 
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public int AdministratorId { get; set; } // FK 

        // Navigation Property
        public Administrator Administrator { get; set; } = null!;

    }
}