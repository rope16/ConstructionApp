using ConstructionApp.Dtos.Project;
using ConstructionApp.Interfaces.ProjectInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("createProject")]
        public async Task<ActionResult<ProjectDetailsDto>> CreateProject(CreateProjectDto dto, [FromServices] IProjectService service)
        {
            var result = await service.CreateProject(dto);

            return Ok(result);
        }

        [HttpGet]
        [Route("getAllProjects")]
        public async Task<ActionResult<List<ProjectDetailsDto>>> GetAllProjects([FromServices] IProjectService service)
        {
            var response = await service.GetAllProjects();
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{projectId}/deleteProject")]
        public async Task<ActionResult<bool>> DeleteProject([FromRoute] Guid projectId, [FromServices] IProjectService service)
        {
            var response = await service.DeleteProject(projectId);
            return Ok(response);
        }

        [HttpPut]
        [Route("{projectId}/updateProjectStatus")]
        public async Task<ActionResult<ProjectDetailsDto>> UpdateProjectStatus(
            [FromRoute] Guid projectId,
            [FromQuery] string status,
            [FromServices] IProjectService service)
        {
            var response = await service.UpdateProjectStatus(projectId, status);

            return response;
        }

        [HttpGet]
        [Route("{projectId}/getProjectWithTasks")]
        public async Task<ActionResult<ProjectDetailsDto>> GetProjectWithTasks([FromRoute] Guid projectId, [FromServices] IProjectService service)
        {
            var response = await service.GetProjectWithTasks(projectId);

            return Ok(response);
        }

        [HttpGet]
        [Route("projectCount")]
        public async Task<ActionResult<int>> GetProjectCount([FromServices] IProjectService service)
        {
            var response = await service.GetProjectCount();
            return Ok(response);
        }

        [HttpGet]
        [Route("getUserProjects")]
        public async Task<ActionResult<List<ProjectDetailsDto>>> GetUserProjects(
            [FromHeader] Guid userId,
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromServices] IProjectService service)
        {
            var response = await service.GetUserProjects(userId, pageNumber, pageSize);
            return Ok(response);
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<SearchProjectResponseDto>> SearchProjects([FromQuery] ProjectFilterDto dto, [FromServices] IProjectService service)
        {
            var reponse = await service.SearchProjects(dto);

            return Ok(reponse);
        }

        [HttpPut]
        [Route("editProject")]
        public async Task<ActionResult<ProjectDetailsDto>> EditProject(
            [FromRoute] Guid projectId,
            [FromBody] EditProjectDto dto,
            [FromServices] IProjectService service)
        {
            var response = await service.EditProject(dto);

            return Ok(response);
        }
    }
}
