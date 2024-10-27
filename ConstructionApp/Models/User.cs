using ConstructionApp.Constants;

namespace ConstructionApp.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public static User CreateUser(
            string firstName,
            string lastName,
            string email,
            string password,
            string role)
        {
            return new User
            {
                UserId = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                Role = role,
                IsActive = true
            };
        }

        public void DeleteUser()
        {
            IsActive = false;
        }
    }
}
