using ConstructionApp.Models;

namespace ConstructionApp.Interfaces.AuthInterfaces
{
    public interface ITokenProviderServices
    {
        public string CreateToken(ConstructionApp.Models.User user);
    }
}
