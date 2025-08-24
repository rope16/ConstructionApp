using ConstructionApp.Dtos.User;
using ConstructionApp.Interfaces.User;
using ConstructionApp.Interfaces.UserInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Route("createUser")]
        public async Task<ActionResult<UserDetailsDto>> CreateUser([FromBody] CreateUserDto dto, [FromServices] IUserService _userService)
        {
            var response = await _userService.CreateUser(dto);

            return Ok(response);
        }

        [HttpGet]
        [Route("getAllUsers")]
        public async Task<ActionResult<List<UserDetailsDto>>> GetAllUsers([FromServices] IUserService _userService)
        {
            var response = await _userService.GetAllUsers();

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{userId}/deleteUser")]
        public async Task<ActionResult<bool>> DeleteUser([FromRoute] Guid userId, [FromServices] IUserService _userService)
        {
            var response = await _userService.DeleteUser(userId);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{userId}/updateUserRole")]
        public async Task<ActionResult<UserDetailsDto>> UpdateUserRole([FromRoute] Guid userId, [FromQuery] string role, [FromServices] IUserService _userService)
        {
            var response = await _userService.UpdateUserRole(userId, role);

            return Ok(response);
        }

        [HttpPut]
        [Route("{userId}/editUser")]
        public async Task<ActionResult<UserDetailsDto>> EditUser([FromRoute] Guid userId, [FromBody] EditUserDto dto, [FromServices] IUserService _userService)
        {
            var response = await _userService.EditUser(dto);

            return Ok(response);
        }

        [HttpGet]
        [Route("{userId}/getProfile")]
        public async Task<ActionResult<UserDetailsDto>> GetProfile([FromRoute] Guid userId, [FromServices] IUserService _userService)
        {
            var response = await _userService.GetUserProfile(userId);

            return Ok(response);
        }

        [HttpGet]
        [Route("searchUsers")]
        public async Task<ActionResult<UserSearchResponseDto>> SearchUsers(
            [FromQuery] int pageSize,
            [FromQuery] int page,
            [FromServices] IUserService _userService,
            [FromQuery] string? query = null)
        {
            var response = await _userService.SearchUsers(page, pageSize, query);

            return response;
        }
    }
}
