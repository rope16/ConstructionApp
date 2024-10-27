using ConstructionApp.Dtos.User;
using ConstructionApp.Interfaces.User;
using ConstructionApp.Interfaces.UserInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Route("createUser")]
        public async Task<ActionResult<UserDetailsDto>> CreateUser(CreateUserDto dto, [FromServices] IUserService _userService)
        {
            var response = await _userService.CreateUser(dto);

            return response;
        }

        [HttpGet]
        [Route("getAllUsers")]
        public async Task<ActionResult<List<UserDetailsDto>>> GetAllUsers([FromServices] IUserService _userService)
        {
            var response = await _userService.GetAllUsers();

            return response;
        }

        [HttpDelete]
        [Route("{userId}/deleteUser")]
        public async Task<ActionResult<bool>> DeleteUser([FromRoute] Guid userId, [FromServices] IUserService _userService)
        {
            var result = await _userService.DeleteUser(userId);

            return result;
        }
    }
}
