using TallerAutonova.Domain.Entities;

namespace TallerAutonova.Domain.Services.FactoryMethod
{
    public abstract class UsuarioFactory
    {
        public abstract User createUsuario();
    }
}