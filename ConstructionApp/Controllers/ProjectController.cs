using ConstructionApp.Dtos.Project;
using ConstructionApp.Interfaces.ProjectInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
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
            var response= await service.GetAllProjects();
            return Ok(response);
        }
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
    }
}
