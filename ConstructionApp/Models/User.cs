namespace ConstructionApp.Models
{
    public enum UserRoles
    {
        Admin,
        User
    }
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
            string password)
        {
            return new User
            {
                UserId = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                Role = UserRoles.User.ToString(),
                IsActive = true
            };
        }

        public void DeleteUser()
        {
            IsActive = false;
        }

        public void UpdateUserRole(string role)
        {
            Role = role;
        }
    }
}
