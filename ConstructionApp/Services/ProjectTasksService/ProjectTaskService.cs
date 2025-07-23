using ConstructionApp.Dtos.Project;
using ConstructionApp.Dtos.ProjectTask;
using ConstructionApp.Dtos.User;
using ConstructionApp.Interfaces.ProjectTasksInterface;
using ConstructionApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace ConstructionApp.Services.ProjectTasksService
{
    public class ProjectTaskService : IProjectTaskInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger <ProjectTaskService> _logger;

        public ProjectTaskService(ApplicationDbContext context, ILogger<ProjectTaskService> logger )
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ProjectTaskDetailsDto> CreateProjectTask(ProjectTaskCreateDto dto)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == dto.ProjectId);

            string base64Image = "";

            if (project is null)
            {
                _logger.LogInformation("Project with provided ID wasn't found");
                return new ProjectTaskDetailsDto();
            }

            var newProjectTask = ProjectTask.CreateProjectTask(dto.Note, dto.StartDate, dto.EndDate, dto.ProjectId);

            if (dto.ProjectPhoto != null && dto.ProjectPhoto.Length != 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await dto.ProjectPhoto.CopyToAsync(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();
                    base64Image = Convert.ToBase64String(imageBytes);
                    newProjectTask.TaskPhoto = base64Image;
                }
            }

            var response = new ProjectTaskDetailsDto
            {
                ProjectTaskId = newProjectTask.ProjectTaskId,
                Note = newProjectTask.Note,
                Status = newProjectTask.Status,
                StartDate = newProjectTask.StartDate,
                EndDate = newProjectTask.EndDate,
                ProjectDetails = new ProjectDetailsDto
                {
                    ProjectId = project.ProjectId,
                    Note = project.Note,
                    Name = project.Name,
                    Status = project.Status,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    ConstructionSiteId = project.ConstructionSiteId
                }
            };

            _context.Add(newProjectTask);

            await _context.SaveChangesAsync();

            return response;
        }

        public async Task<ProjectTaskDetailsDto> UploadProjectTaskPhoto(IFormFile image, Guid projectTaskId)
        {
            var projectTask = await _context.ProjectTasks
                .Include(pt => pt.Project)
                .FirstOrDefaultAsync(pt => pt.ProjectTaskId == projectTaskId);

            if (projectTask is null)
            {
                _logger.LogInformation("Project task with provided was not found.");
                return new ProjectTaskDetailsDto();
            }

            string base64Image = "";

            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();
                base64Image = Convert.ToBase64String(imageBytes);
            }

            projectTask.TaskPhoto = base64Image;

            await _context.SaveChangesAsync();

            var response = new ProjectTaskDetailsDto
            {
                ProjectTaskId = projectTask.ProjectTaskId,
                Note = projectTask.Note,
                Status = projectTask.Status,
                StartDate = projectTask.StartDate,
                EndDate = projectTask.EndDate,
                ProjectDetails = new ProjectDetailsDto
                {
                    ProjectId = projectTask.Project.ProjectId,
                    Note = projectTask.Project.Note,
                    Name = projectTask.Project.Name,
                    Status = projectTask.Project.Status,
                    StartDate = projectTask.Project.StartDate,
                    EndDate = projectTask.Project.EndDate,
                    ConstructionSiteId = projectTask.Project.ConstructionSiteId
                }
            };

            return response;
        }
        public async Task<List<ProjectTaskDetailsDto>> GetAllProjectTasks()
        {
        var projectTasks = await _context.ProjectTasks.Include(pt => pt.Project).ToListAsync();

            if (!projectTasks.Any())
            {
                _logger.LogInformation("No project tasks");
                return new List<ProjectTaskDetailsDto>(); 
            }
            var response = projectTasks.Select(pt => new  ProjectTaskDetailsDto
            {
                ProjectTaskId =pt.ProjectTaskId,
                Note =pt.Note,
                Status =pt.Status,
                StartDate =pt.StartDate,
                EndDate =pt.EndDate,
                ProjectDetails = new ProjectDetailsDto
                {
                    ProjectId =pt.Project.ProjectId,
                    Note =pt.Project.Note,
                    ConstructionSiteId = pt.Project.ConstructionSiteId,
                    Name =pt.Project.Name,
                    Status =pt.Project.Status,
                    StartDate =pt.Project.StartDate,
                    EndDate =pt.Project.EndDate,
                    
                }
            }).ToList();
            return response;
        }
        public async Task<bool> DeleteProjectTask(Guid projectId)
        {
            var projectTask = await _context.ProjectTasks.FirstOrDefaultAsync(pt => pt.ProjectTaskId == projectId);

            if (projectTask == null)
            {
                _logger.LogInformation("Project task with provided Id was not found");
                return false;
            }

            _context.ProjectTasks.Remove(projectTask);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<UserDetailsDto>> GetProjectTaskUsers(Guid projectTaskId)
        {
        var projectTaskUsers = await _context.UserTasks.Include(u=>u.User).Where(p=>p.ProjectTaskId == projectTaskId).ToListAsync();

            if (!projectTaskUsers.Any())
            {
                _logger.LogInformation("No users assigned to this project task");
                return new List<UserDetailsDto>(); 
            }
            var response = projectTaskUsers.Select(u => new UserDetailsDto
            {
                Email = u.User.Email,
                FirstName = u.User.FirstName,
                LastName = u.User.LastName,
                IsActive = u.User.IsActive,
                Role = u.User.Role,
                UserId = u.User.UserId
            }).ToList();

            return response;
        }

    }
}
