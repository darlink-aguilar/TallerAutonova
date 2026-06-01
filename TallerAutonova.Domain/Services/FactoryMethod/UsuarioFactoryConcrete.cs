using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Enums;
using TallerAutonova.Domain.Services.FactoryMethod;

namespace TallerAutonova.Domain.Services.FactoryMethod
{
    public class UsuarioFactoryConcrete
    {
        private static UsuarioFactory? _factory;

        private static UsuarioFactory GetFactory(UserRole role)
        {
            return role switch
            {
                UserRole.Mechanic => new MechanicFactoryConcrete(),
                UserRole.Receptionist => new ReceptionistFactoryConcrete(),
                _ => throw new ArgumentException($"Rol {role} no válido")
            };
        }

        public User registerNewEmployee(string login, string password, string name, int administratorId, UserRole role)
        {
            _factory = GetFactory(role);
            var user = _factory.createUsuario();

            user.Email = login;
            user.Password = password;
            user.Name = name;
            user.AdministratorId = administratorId;
            user.Role = role;

            return user;
        }
    }
}