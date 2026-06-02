using TallerAutonova.Domain.Enums;

namespace TallerAutonova.API.DTOs.Request
{
    public class CreateUserRequestDTO
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int AdministratorId { get; set; }
        public UserRole Role { get; set; }
    }
}