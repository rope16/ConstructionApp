using ConstructionApp.Dtos.Authentication;
using ConstructionApp.Interfaces.AuthInterfaces;
using ConstructionApp.Interfaces.User;
using Microsoft.EntityFrameworkCore;

namespace ConstructionApp.Services.AuthServices
{
    public class UserAuthService : IUserAuthServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserPasswordHasherService _passwordHasher;
        private readonly ITokenProviderServices _tokenProvider;

        public UserAuthService(ApplicationDbContext context, IUserPasswordHasherService passwordHasher, ITokenProviderServices tokerProvider)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenProvider = tokerProvider;
        }

        public async Task<string> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email ==  dto.Email);

            if (user == null)
            {
                throw new Exception("User with provided email does not exist.");
            }

            var valid = _passwordHasher.VerifyPassword(dto.Password, user.Password);

            if (!valid)
            {
                throw new Exception("Password was incorrect");
            }

            var token = _tokenProvider.CreateToken(user);

            return token;
        }
    }
}
