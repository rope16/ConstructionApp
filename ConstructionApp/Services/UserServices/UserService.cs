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

            if (!Enum.TryParse(typeof(UserRoles), dto.Role, out var validRole))
            {
                _logger.LogInformation("User role does not exist.");
                return new UserDetailsDto();
            }

            var newUser = User.CreateUser(dto.FirstName, dto.LastName, dto.Email, hashedPassword, validRole.ToString());

            _context.Add(newUser);

            var response = new UserDetailsDto
            {
                UserId = newUser.UserId,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                IsActive = newUser.IsActive,
                Role = validRole.ToString()
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

        public async Task<UserDetailsDto> UpdateUserRole(Guid userId, string role)
        {
            if (!Enum.TryParse(typeof(UserRoles), role, out var validRole))
            {
                _logger.LogInformation("Provided role doesn't exist.");
                return new UserDetailsDto();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user is null)
            {
                _logger.LogInformation("User with provided id doesn't exist.");
                return new UserDetailsDto();
            }

            user.UpdateUserRole(validRole.ToString());

            var response = new UserDetailsDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
            };

            await _context.SaveChangesAsync();

            return response;
        }

        public async Task<UserDetailsDto> EditUser(EditUserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == dto.UserId);

            if (user == null)
                throw new Exception("Failed to update user, user not found.");

            user.EditUser(dto.FirstName, dto.LastName, dto.Email);

            await _context.SaveChangesAsync();

            var response = new UserDetailsDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
            };

            return response;
        }

        public async Task<UserDetailsDto> GetUserProfile(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                throw new Exception("Failed to update user, user not found.");

            var response = new UserDetailsDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
            };

            return response;
        }

        public async Task<UserSearchResponseDto> SearchUsers(int page, int pageSize, string? query = null)
        {
            var searchQuery = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query) && !string.IsNullOrEmpty(query))
            {
                searchQuery = searchQuery.Where(u =>
                    EF.Functions.ILike(u.FirstName, $"%{query}%") ||
                    EF.Functions.ILike(u.LastName, $"%{query}%") ||
                    EF.Functions.ILike(u.Email, $"%{query}%"));
            }

            var totalCount = await searchQuery.CountAsync();

            var users = await searchQuery
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.CreatedAtUtc)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedUsers = users.Select(u => new UserDetailsDto
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role,
                IsActive = u.IsActive,
            }).ToList();

            var response = new UserSearchResponseDto
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Users = mappedUsers,
            };

            return response;
        }
    }
}
