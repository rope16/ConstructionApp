using ConstructionApp.Dtos.ProjectTask;
using ConstructionApp.Dtos.User;
using ConstructionApp.Interfaces.ProjectTasksInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTaskController : ControllerBase
    {
        [HttpPost]
        [Route("createProjectTask")]
        public async Task<ActionResult<ProjectTaskDetailsDto>> CreateProjectTask(
        [FromForm] ProjectTaskCreateDto dto,
        [FromServices] IProjectTaskInterface service)
        {
            var result = await service.CreateProjectTask(dto);

            return Ok(result);
        }

        [HttpPost]
        [Route("uploadImage")]
        public async Task<ActionResult<ProjectTaskDetailsDto>> UploadProjectPhoto([FromForm] ProjectTaskUploadImageDto dto,
                                                                                  [FromServices] IProjectTaskInterface service)
        {
            var result = await service.UploadProjectTaskPhoto(dto.Image, dto.ProjectTaskId);

            return Ok(result);
        }
        [HttpGet]
        [Route("getAllProjectTask")]
        public async Task<ActionResult<List<ProjectTaskDetailsDto>>> GetAllProjectTasks([FromServices] IProjectTaskInterface service)
        {
            var result = await service.GetAllProjectTasks();

            return Ok(result);
        }
        [HttpDelete]
        [Route("{projectTaskId}/deleteProjectTask")]

        public async Task<ActionResult<bool>> DeleteProjectTask([FromRoute] Guid projectTaskId, [FromServices] IProjectTaskInterface service)
        {
            var result = await service.DeleteProjectTask(projectTaskId);
            return Ok(result);
        }
        [HttpGet]
        [Route("{projectTaskId}/users")]

        public async Task<ActionResult<List<UserDetailsDto>>> GetProjectTaskUsers([FromRoute] Guid projectTaskId, [FromServices] IProjectTaskInterface service)
        {
            var result = await service.GetProjectTaskUsers(projectTaskId);

            return Ok(result);
        }
    }
}
