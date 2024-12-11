using ConstructionApp.Dtos.Authentication;
using ConstructionApp.Interfaces.AuthInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Route("/login")]
        public async Task<ActionResult<string>> LoginUser([FromBody]LoginDto dto, [FromServices] IUserAuthServices authService)
        {
            var result = await authService.Login(dto);

            return Ok(result);
        }
    }
}
