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
        public DateTime CreatedAtUtc { get; set; }

        public ICollection<UserTask> UserTasks { get; set; } = new HashSet<UserTask>();

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
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
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

        public void EditUser(string firstName, string lastName, string email)
        {
            if (!string.IsNullOrEmpty(firstName))
            {
                FirstName = firstName;
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                LastName = lastName;
            }

            if (string.IsNullOrEmpty(email)) 
            {
                Email = email;
            }
        }
    }
}
