using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.Services.FactoryMethod
{
    public class ReclamoFactory
    {
        public string Game { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public void AssignPermission(int id, User usuario)
        {
            Console.WriteLine($"[ReclamoFactory] Asignando permiso {id} a usuario {usuario.Name}");
        }
    }
}