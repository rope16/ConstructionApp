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
            throw new NotImplementedException();
        }
    }
}
