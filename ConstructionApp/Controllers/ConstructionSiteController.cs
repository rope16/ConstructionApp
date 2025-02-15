﻿using ConstructionApp.Dtos;
using ConstructionApp.Dtos.ConstructionSite;
using ConstructionApp.Interfaces.ConstructionSite;
using ConstructionApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConstructionApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConstructionSiteController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("createConstructionSite")]
        public async Task<ActionResult<ConstructionSiteDetailDto>> CreateConstructionSite(
            ConstructionSiteCreateDto dto,
            [FromServices] IConstructionSiteService service)
        {
            var response = await service.CreateConstructionSite(dto);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{constructionSiteId}/deleteConstructionSite")]
        public async Task<ActionResult<bool>> DeleteConstructionSite(
            [FromRoute] Guid constructionSiteId,
            [FromServices] IConstructionSiteService service)
        {
            var response = await service.DeleteConstructionSite(constructionSiteId);

            return Ok(response);
        }

        [HttpGet]
        [Route("/getAllCOnstructionSites")]
        public async Task<ActionResult<List<ConstructionSiteDetailDto>>> GetAllConstructionSites([FromServices] IConstructionSiteService service)
        {
            var response = await service.GetAllConstructionSites();

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{constructionSiteId}/editConstructionSite")]
        public async Task<ActionResult<ConstructionSiteDetailDto>> EditConstructionSite([FromRoute] Guid constructionSiteId, [FromBody] ConstructionSiteEditDto dto, [FromServices] IConstructionSiteService service)
        {
            var response = await service.EditConstructionSite(constructionSiteId, dto);
            return response;
        }
    }
}
