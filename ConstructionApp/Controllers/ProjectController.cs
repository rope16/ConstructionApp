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
    }
}
