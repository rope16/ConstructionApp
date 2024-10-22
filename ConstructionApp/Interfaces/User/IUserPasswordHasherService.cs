namespace ConstructionApp.Interfaces.User
{
    public interface IUserPasswordHasherService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
