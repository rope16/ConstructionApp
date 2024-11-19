using ConstructionApp.Dtos.ProjectTask;
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
        ProjectTaskCreateDto dto,
        [FromServices] IProjectTaskInterface service)
        {
            var result = await service.CreateProjectTask(dto);

            return Ok(result);
        }
    }
}
