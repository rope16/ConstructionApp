using ConstructionApp.Dtos.User;
using ConstructionApp.Interfaces.User;
using ConstructionApp.Interfaces.UserInterfaces;
using ConstructionApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionApp.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly IUserPasswordHasherService _passwordHasher;

        public UserService(
            ApplicationDbContext context,
            ILogger<UserService> logger,
            IUserPasswordHasherService passwordHasher)
        {
            _context = context;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDetailsDto> CreateUser(CreateUserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email ==  dto.Email);

            if (user is not null)
            {
                _logger.LogInformation("User with provided email already exists.");
                return new UserDetailsDto();
            }

            var hashedPassword = _passwordHasher.HashPassword(dto.Password);

            var newUser = User.CreateUser(dto.FirstName, dto.LastName, dto.Email, hashedPassword, dto.Role);

            _context.Add(newUser);

            var response = new UserDetailsDto
            {
                UserId = newUser.UserId,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                IsActive = newUser.IsActive,
                Role = newUser.Role
            };

            await _context.SaveChangesAsync();

            return response;
        }

        public async Task<List<UserDetailsDto>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            if (users.Count() == 0)
            {
                _logger.LogInformation("No users in the database.");
                return new List<UserDetailsDto>();
            }

            var response = users.Select(u => new UserDetailsDto
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                IsActive = u.IsActive,
                Role = u.Role
            }).ToList();

            return response;
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user is null)
            {
                _logger.LogInformation("User with provided id doesn't exist.");
                return false;
            }

            user.DeleteUser();

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
