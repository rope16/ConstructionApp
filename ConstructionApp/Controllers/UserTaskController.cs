using ConstructionApp.Dtos.UserTask;
using ConstructionApp.Interfaces.UserTaskInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserTaskController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<UserTaskDetailsDto>> CreateUserTask(UserTaskCreateDto dto, [FromServices] UserTaskServices service)
        {
            var response = await service.CreateUserTask(dto);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{userTaskId}")]
        public async Task<ActionResult<bool>> DeleteUserTask([FromRoute] Guid userTaskId, [FromServices] UserTaskServices service)
        {
            var response = await service.DeleteUserTask(userTaskId);
            return Ok(response);
        }

        [HttpGet]
        [Route("getUserTasks")]
        public async Task<ActionResult<List<UserTaskDetailsCardDto>>> GetUserTasks(
            [FromHeader] Guid userId,
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromServices] UserTaskServices service)
        {
            var response = await service.GetUserTasks(userId, pageNumber, pageSize);
            return Ok(response);
        }
    }
}
