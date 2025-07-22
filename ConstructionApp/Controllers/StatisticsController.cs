using ConstructionApp.Dtos.Stats;
using ConstructionApp.Interfaces.StatsInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        [HttpGet]
        [Route("getGeneralStats")]
        public async Task<ActionResult<StatsDto>> GetGeneralStats([FromServices] IStatisticsInterface service)
        {
            var response = await service.GetGeneralStatistics();

            return Ok(response);
        }
    }
}
