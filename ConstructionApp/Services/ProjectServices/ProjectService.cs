using ConstructionApp.Dtos.Project;
using ConstructionApp.Interfaces.ProjectInterfaces;
using ConstructionApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionApp.Services.ProjectServices
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(ApplicationDbContext context, ILogger<ProjectService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ProjectDetailsDto> CreateProject(CreateProjectDto dto)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Name == dto.Name); 

            if (project is not null)
            {
                _logger.LogInformation("Project already exists.");
                return new ProjectDetailsDto();
            }

            var newProject = Project.CreateProject(dto.Name, dto.Note, dto.StartDate, dto.EndDate, dto.Status, dto.ConstructionSiteId);

            _context.Add(newProject);

            var response = new ProjectDetailsDto
            {
                ProjectId = newProject.ProjectId,
                Name = newProject.Name,
                Note = newProject.Note,
                StartDate = newProject.StartDate,
                EndDate = newProject.EndDate,
                Status = newProject.Status,
                ConstructionSiteId = newProject.ConstructionSiteId,
            };

            await _context.SaveChangesAsync();

            return response;
        }
    }
}
