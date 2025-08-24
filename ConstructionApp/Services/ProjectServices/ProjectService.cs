using ConstructionApp.Dtos.Project;
using ConstructionApp.Dtos.ProjectTask;
using ConstructionApp.Interfaces.ProjectInterfaces;
using ConstructionApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

            var projectTasks = await _context.ProjectTasks.Where(p => p.ProjectId == projectId).ToListAsync();

            List<ProjectTaskDetailsDto> tasks = new List<ProjectTaskDetailsDto>();

            foreach (var task in projectTasks)
            {
                var projectTask = new ProjectTaskDetailsDto
                {
                    ProjectTaskId = task.ProjectTaskId,
                    Title = task.Title,
                    Note = task.Note,
                    StartDate = task.StartDate,
                    EndDate = task.EndDate,
                    Status = task.Status,
                };

                tasks.Add(projectTask);
            }

            var response = new ProjectDetailsDto
            {
                ProjectId = project.ProjectId,
                Name = project.Name,
                Note = project.Note,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.Status,
                ProjectTasks = tasks,
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
            var project = await _context.Projects
                .Include(p => p.ConstructionSite)
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);

            if (project is null)
            {
                _logger.LogInformation("Project was not found");
                return new ProjectDetailsDto();
            }

            var projectTasks = await _context.ProjectTasks.Where(p => p.ProjectId == projectId).ToListAsync();

            List<ProjectTaskDetailsDto> tasks = new List<ProjectTaskDetailsDto>();

            foreach (var task in projectTasks)
            {
                var projectTask = new ProjectTaskDetailsDto
                {
                    ProjectTaskId = task.ProjectTaskId,
                    Title = task.Title,
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
                ProjectTasks = tasks,
                ConstructionSiteDetails = new Dtos.ConstructionSite.ConstructionSiteDetailDto
                {
                    ConstructionSiteId = project.ConstructionSite.ConstructionSiteId,
                    Address = project.ConstructionSite.Address,
                    Contractor = project.ConstructionSite.Contractor,
                    Investor = project.ConstructionSite.Investor
                }
            };

            return result;
        }

        public async Task<int> GetProjectCount()
        {
            var count = await _context.Projects.CountAsync();
            return count;
        }

        public async Task<List<ProjectDetailsDto>> GetUserProjects(Guid userId, int pageNumber, int pageSize)
        {
            var userProjects = await _context.UserTasks
                .Where(ut => ut.UserId == userId)
                .Include(ut => ut.ProjectTask)
                .ThenInclude(pt => pt.Project)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (!userProjects.Any())
            {
                _logger.LogInformation("No projects found for the user");
                return new List<ProjectDetailsDto>();
            }

            var response = userProjects.Select(ut => new ProjectDetailsDto
            {
                ProjectId = ut.ProjectTask.Project.ProjectId,
                Name = ut.ProjectTask.Project.Name,
                Note = ut.ProjectTask.Project.Note,
                StartDate = ut.ProjectTask.Project.StartDate,
                EndDate = ut.ProjectTask.Project.EndDate,
                Status = ut.ProjectTask.Project.Status,
                ConstructionSiteId = ut.ProjectTask.Project.ConstructionSiteId
            }).ToList();

            return response;
        }

        public async Task<SearchProjectResponseDto> SearchProjects(ProjectFilterDto filterDto)
        {
            var query = _context.Projects
                .Include(p => p.ConstructionSite)
                .Include(p => p.Tasks)
                    .ThenInclude(pt => pt.ProjectTaskUsers)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterDto.Search))
            {
                var searchLower = filterDto.Search.ToLower();
                query = query.Where(p =>
                    p.Name.Contains(searchLower) ||
                    p.Note.Contains(searchLower));
            }

            if (!string.IsNullOrEmpty(filterDto.Status) && Enum.TryParse<ProjectStatus>(filterDto.Status, true, out var statusEnum))
            {
                query = query.Where(p => p.Status == filterDto.Status);
            }

            if (filterDto.StartFrom.HasValue)
            {
                var startUtc = DateTime.SpecifyKind(filterDto.StartFrom.Value, DateTimeKind.Utc);
                query = query.Where(p => p.StartDate >= startUtc);
            }

            if (filterDto.EndTo.HasValue)
            {
                var endUtc = DateTime.SpecifyKind(filterDto.EndTo.Value, DateTimeKind.Utc);
                query = query.Where(p => p.EndDate <= endUtc);
            }

            if (filterDto.UserId.HasValue && filterDto.UserId != Guid.Empty)
            {
                query = query.Where(pt =>
                    pt.Tasks.Any(t =>
                        t.ProjectTaskUsers.Any(ptu => ptu.UserId == filterDto.UserId.Value)));
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.StartDate)
                .Skip((filterDto.Page - 1) * filterDto.PageSize)
                .Take(filterDto.PageSize)
                .ToListAsync();

            var projectsResponse = items.Select(p => new ProjectDetailsDto
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

            var response = new SearchProjectResponseDto { 
                Projects = projectsResponse,
                Count = total,
                Page = filterDto.Page,
                PageSize = filterDto.PageSize
            };

            return response;
        }

        public async Task<ProjectDetailsDto> EditProject(EditProjectDto dto)
        {
            var project = await _context.Projects
                .Include(p => p.ConstructionSite)
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.ProjectId == dto.ProjectId);

            if (project is null)
            {
                _logger.LogInformation("Project with provided Id doesn't exist");
                return new ProjectDetailsDto();
            }

            project.EditProject(dto.Name, dto.Note, dto.StartDate, dto.EndDate);

            await _context.SaveChangesAsync();

            var response = new ProjectDetailsDto
            {
                ProjectId = project.ProjectId,
                Name = project.Name,
                Note = project.Note,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.Status,
                ProjectTasks = [.. project.Tasks.Select(task => new ProjectTaskDetailsDto
                {
                    ProjectTaskId = task.ProjectTaskId,
                    Title = task.Title,
                    Note = task.Note,
                    StartDate = task.StartDate,
                    EndDate = task.EndDate,
                    Status = task.Status,
                })],
                ConstructionSiteDetails = new Dtos.ConstructionSite.ConstructionSiteDetailDto
                {
                    ConstructionSiteId = project.ConstructionSite.ConstructionSiteId,
                    Address = project.ConstructionSite.Address,
                    Contractor = project.ConstructionSite.Contractor,
                    Investor = project.ConstructionSite.Investor
                }
            };

            return response;
        }
    }
}
