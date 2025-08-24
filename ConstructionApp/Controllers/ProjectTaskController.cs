using ConstructionApp.Dtos.ProjectTask;
using ConstructionApp.Dtos.User;
using ConstructionApp.Interfaces.ProjectTasksInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTaskController : ControllerBase
    {
        [HttpPost]
        [Route("createProjectTask")]
        public async Task<ActionResult<ProjectTaskDetailsDto>> CreateProjectTask(
        [FromBody] ProjectTaskCreateDto dto,
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

        [Authorize(Roles = "Admin")]
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

        [HttpGet]
        [Route("{projectTaskId}/details")]
        public async Task<ActionResult<ProjectTaskDetailsDto>> GetProjectTaskDetails([FromRoute] Guid projectTaskId, [FromServices] IProjectTaskInterface service)
        {
            var result = await service.GetProjectTaskDetails(projectTaskId);

            return Ok(result);
        }

        [HttpPut]
        [Route("{projectTaskId}/updateStatus")]
        public async Task<ActionResult<ProjectTaskDetailsDto>> UpdateStatus(
            [FromRoute] Guid projectTaskId,
            [FromQuery] string status,
            [FromServices] IProjectTaskInterface service)
        {
            var response = await service.UpdateProjectTaskStatus(projectTaskId, status);
            
            return Ok(response);
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<ProjectTaskSearchResponseDto>> Search([FromQuery] ProjectTaskFilterDto dto, [FromServices] IProjectTaskInterface service)
        {
            var response = await service.SearchProjectTasks(dto);

            return response;
        }

        [HttpPut]
        [Route("edit")]
        public async Task<ActionResult<ProjectTaskDetailsDto>> EditProjectTask([FromBody] ProjectTaskEditDto dto, [FromServices] IProjectTaskInterface service)
        {
            var response = await service.EditProjectTask(dto);

            return Ok(response);
        }
    }
}
