using ConstructionApp.Dtos.Authentication;

namespace ConstructionApp.Interfaces.AuthInterfaces
{
    public interface IUserAuthServices
    {
        Task<string> Login(LoginDto dto);
    }
}
