using ConstructionApp.Dtos.User;

namespace ConstructionApp.Interfaces.UserInterfaces
{
    public interface IUserService
    {
        Task<UserDetailsDto> CreateUser (CreateUserDto dto);
        Task<List<UserDetailsDto>> GetAllUsers ();
        Task<bool> DeleteUser(Guid userId);
    }
}
