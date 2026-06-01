namespace TallerAutonova.Domain.Entities
{
    public class Administrator : AuditBase
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty; // Nombre de usuario 
        public string Password { get; set; } = string.Empty;

        // Navigation Property
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Part> Parts { get; set; } = new List<Part>();

    }
}