using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Services.FactoryMethod;

namespace TallerAutonova.Domain.Services.FactoryMethod
{
    public class ReceptionistFactoryConcrete : UsuarioFactory
    {
        public override User createUsuario()
        {
            return new Receptionist();
        }
    }
}