using ConstructionApp.Dtos.ConstructionSite;
using ConstructionApp.Dtos;
using ConstructionApp.Interfaces.ConstructionSite;
using Microsoft.EntityFrameworkCore;
using ConstructionApp.Models;
using ConstructionApp.Controllers;

namespace ConstructionApp.Services.ConstructionSiteService
{
    public class ConstructionSiteService : IConstructionSiteService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ConstructionSiteService> _logger;

        public ConstructionSiteService(ApplicationDbContext context, ILogger<ConstructionSiteService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ConstructionSiteDetailDto> CreateConstructionSite(ConstructionSiteCreateDto dto)
        {
            var constructionSite = await _context.ConstructionSites
                .FirstOrDefaultAsync(cs => cs.Contractor == dto.Contractor && cs.Address == dto.Address && cs.Investor == dto.Investor);

            if (constructionSite != null)
            {
                _logger.LogInformation("Construction site already exists.");
                return new ConstructionSiteDetailDto();
            }

            var contructionSite = ConstructionSite.CreateConstructionSite(dto.Address, dto.Contractor, dto.Investor);

            var response = new ConstructionSiteDetailDto
            {
                ConstructionSiteId = contructionSite.ConstructionSiteId,
                Contractor = contructionSite.Contractor,
                Investor = contructionSite.Investor,
                Address = contructionSite.Address
            };

            _context.ConstructionSites.Add(contructionSite);

            await _context.SaveChangesAsync();

            return response;
        }

        public async Task<bool> DeleteConstructionSite(Guid constructionSiteId)
        {
            var constructionSite = await _context.ConstructionSites.FirstOrDefaultAsync(cs => cs.ConstructionSiteId == constructionSiteId);

            if (constructionSite is null)
            {
                _logger.LogInformation("Construction site with provided Id doesn't exist.");
                return false;
            }

            _context.ConstructionSites.Remove(constructionSite);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ConstructionSiteDetailDto>> GetAllConstructionSites()
        {
            var constructionSites = await _context.ConstructionSites.ToListAsync();

            if (constructionSites.Count() == 0)
            {
                _logger.LogInformation("No construciton sites in the database.");
                return new List<ConstructionSiteDetailDto>();
            }

            var response = constructionSites.Select(cs => new ConstructionSiteDetailDto
            {
                ConstructionSiteId = cs.ConstructionSiteId,
                Address = cs.Address,
                Contractor = cs.Contractor,
                Investor = cs.Investor,
            }).ToList();

            return response;
        }
        public async Task<ConstructionSiteDetailDto> EditConstructionSite(Guid constructionSiteId, ConstructionSiteEditDto dto)
        {
            var constructionSite = await _context.ConstructionSites.FirstOrDefaultAsync(cs => cs.ConstructionSiteId == constructionSiteId);

            if (constructionSite is null)
            {
                _logger.LogInformation("Construction site with provided Id doesn't exist.");
                return new ConstructionSiteDetailDto();
            }

            constructionSite.Address = dto.Address;
            constructionSite.Investor = dto.Investor;
            constructionSite.Contractor = dto.Contractor;

            await _context.SaveChangesAsync();

            var response = new ConstructionSiteDetailDto
            {
                ConstructionSiteId = constructionSite.ConstructionSiteId,
                Investor = constructionSite.Investor,
                Address = constructionSite.Address,
                Contractor = dto.Contractor,
            };

            return response;
        }

        public async Task<int> GetConstructionSiteCount()
        {
            var count = await _context.ConstructionSites.CountAsync();
            return count;
        }

        public async Task<ConstructionSiteSearchResponseDto> Search(ConstructionSiteFilterDto dto)
        {
            var query = _context.ConstructionSites.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Search))
            {
                var searchToLower = dto.Search.ToLower();
                query = query.Where(cs => 
                    EF.Functions.ILike(cs.Contractor, $"%{searchToLower}%") ||
                    EF.Functions.ILike(cs.Address, $"%{searchToLower}%") ||
                    EF.Functions.ILike(cs.Investor, $"%{searchToLower}%"));
            }

            var totalCount = await query.CountAsync();

            var sites = await query
                .OrderBy(cs => cs.Address)
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync();

            var mappedSites = sites.Select(cs => new ConstructionSiteDetailDto
            {
                ConstructionSiteId = cs.ConstructionSiteId,
                Address = cs.Address,
                Contractor = cs.Contractor,
                Investor = cs.Investor
            }).ToList();

            var response = new ConstructionSiteSearchResponseDto
            {
                Count = totalCount,
                Page = dto.Page,
                PageSize = dto.PageSize,
                Sites = mappedSites
            };

            return response;
        }
    }
}
