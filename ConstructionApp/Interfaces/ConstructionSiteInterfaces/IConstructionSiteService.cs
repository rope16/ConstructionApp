using ConstructionApp.Dtos;
using ConstructionApp.Dtos.ConstructionSite;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionApp.Interfaces.ConstructionSite
{
    public interface IConstructionSiteService
    {
        Task<ConstructionSiteDetailDto> CreateConstructionSite(ConstructionSiteCreateDto dto);
        Task<bool> DeleteConstructionSite(Guid constructionSiteId);
        Task<List<ConstructionSiteDetailDto>> GetAllConstructionSites();
        Task<ConstructionSiteDetailDto> EditConstructionSite(Guid constructionSiteId, ConstructionSiteEditDto dto);
    }
}
