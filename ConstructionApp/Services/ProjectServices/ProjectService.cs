using ConstructionApp.Dtos.Project;
using ConstructionApp.Dtos.ProjectTask;
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

            var newProject = Project.CreateProject(dto.Name, dto.Note, dto.StartDate, dto.EndDate, dto.ConstructionSiteId);

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

        public async Task<List<ProjectDetailsDto>> GetAllProjects()
        {
            var projects = await _context.Projects.Include(p => p.ConstructionSite).ToListAsync();

            if (!projects.Any())
            {
                _logger.LogInformation("NO projects in database");
                return new List<ProjectDetailsDto>();
            }

            var response = projects.Select(p => new ProjectDetailsDto
            {
                ProjectId = p.ProjectId,
                Name = p.Name,
                Note = p.Note,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Status = p.Status,
                ConstructionSiteDetails = new Dtos.ConstructionSite.ConstructionSiteDetailDto
                {
                    ConstructionSiteId = p.ConstructionSite.ConstructionSiteId,
                    Address = p.ConstructionSite.Address,
                    Contractor = p.ConstructionSite.Contractor,
                    Investor = p.ConstructionSite.Investor
                }
            }).ToList();

            return response;
        }
        public async Task<bool> DeleteProject(Guid projectId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == projectId);

            if (project == null)
            {
                _logger.LogInformation("Project with provided Id does not exist");
                return false;
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ProjectDetailsDto> UpdateProjectStatus(Guid projectId, string status)
        {
            if (!Enum.TryParse(typeof(ProjectStatus), status, out var validStatus))
            {
                _logger.LogInformation("Project status doesn't exist.");
                return new ProjectDetailsDto();
            }

            var project = await _context.Projects
                                .Include(p => p.ConstructionSite)
                                .FirstOrDefaultAsync(p => p.ProjectId == projectId);

            if (project is null)
            {
                _logger.LogInformation("Project with provided Id doesn't exist");
                return new ProjectDetailsDto();
            }

            project.UpdateProjectStatus(validStatus.ToString());

            var response = new ProjectDetailsDto
            {
                ProjectId = project.ProjectId,
                Name = project.Name,
                Note = project.Note,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.Status,
                ConstructionSiteDetails = new Dtos.ConstructionSite.ConstructionSiteDetailDto
                {
                    ConstructionSiteId = project.ConstructionSite.ConstructionSiteId,
                    Address = project.ConstructionSite.Address,
                    Contractor = project.ConstructionSite.Contractor,
                    Investor = project.ConstructionSite.Investor
                }
            };

            await _context.SaveChangesAsync();

            return response;
        }
        public async Task<ProjectDetailsDto> GetProjectWithTasks(Guid projectId)
        {
            var project= await _context.Projects.FirstOrDefaultAsync(p=>p.ProjectId == projectId);
            if (project is null)
            {
                _logger.LogInformation("Project was not found");
                return new ProjectDetailsDto();
            }
            var projectTasks=await _context.ProjectTasks.Where(p=>p.ProjectId == projectId).ToListAsync();

            List<ProjectTaskDetailsDto> tasks = new List<ProjectTaskDetailsDto>();

            foreach( var task in projectTasks )
            {
                var projectTask = new ProjectTaskDetailsDto
                {
                    ProjectTaskId = task.ProjectTaskId,
                    Note = task.Note,
                    StartDate = task.StartDate,
                    EndDate = task.EndDate,
                    Status = task.Status,
                };

                tasks.Add(projectTask);
            }

            var result = new ProjectDetailsDto
            {
                ProjectId = project.ProjectId,
                Name = project.Name,
                Note = project.Note,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                ConstructionSiteId = project.ConstructionSiteId,
                Status = project.Status,
                ProjectTasksDto = tasks
            };

            return result;
        }
    }
}
