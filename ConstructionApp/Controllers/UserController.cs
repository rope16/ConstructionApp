using ConstructionApp.Interfaces.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("hashPassword")]
        public string HashPassword(string password, [FromServices] IUserPasswordHasherService _passwordHasher)
        {
            var hashedPassowrd = _passwordHasher.HashPassword(password);
            return hashedPassowrd;
        }

        [HttpGet]
        [Route("verifyPassword")]
        public bool VerifyPassword(string password, string hashedPassword, [FromServices] IUserPasswordHasherService _passwordHasher)
        {
            var result = _passwordHasher.VerifyPassword(password, hashedPassword);

            return result;
        }
    }
}
