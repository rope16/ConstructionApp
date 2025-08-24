using ConstructionApp.Dtos.Project;
using ConstructionApp.Dtos.ProjectTask;
using ConstructionApp.Dtos.User;
using ConstructionApp.Dtos.UserTask;
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

            var newProjectTask = ProjectTask.CreateProjectTask(dto.Title, dto.Note, dto.StartDate, dto.EndDate, dto.ProjectId);

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
                Title = dto.Title,
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
                Title = projectTask.Title,
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
                ProjectTaskId = pt.ProjectTaskId,
                Title = pt.Title,
                Note = pt.Note,
                Status = pt.Status,
                StartDate = pt.StartDate,
                EndDate = pt.EndDate,
                ProjectDetails = new ProjectDetailsDto
                {
                    ProjectId = pt.Project.ProjectId,
                    Note = pt.Project.Note,
                    ConstructionSiteId = pt.Project.ConstructionSiteId,
                    Name = pt.Project.Name,
                    Status = pt.Project.Status,
                    StartDate = pt.Project.StartDate,
                    EndDate = pt.Project.EndDate,
                    
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

        public async Task<ProjectTaskDetailsDto> UpdateProjectTaskStatus(Guid projectTaskId, string status)
        {
            if (!Enum.TryParse(typeof(ProjectTaskStatus), status, out var projectTaskStatus))
            {
                _logger.LogInformation("Project task status doesn't exist.");
                return new ProjectTaskDetailsDto();
            }

            var projectTask = await _context.ProjectTasks
                .Include(pt => pt.Project)
                .FirstOrDefaultAsync(pt => pt.ProjectTaskId == projectTaskId);

            var response = new ProjectTaskDetailsDto
            {
                ProjectTaskId = projectTask.ProjectTaskId,
                Title = projectTask.Title,
                Note = projectTask.Note,
                Status = projectTask.Status,
                StartDate = projectTask.StartDate,
                EndDate = projectTask.EndDate,
                ProjectDetails = new ProjectDetailsDto
                {
                    ProjectId = projectTask.Project.ProjectId,
                    Note = projectTask.Project.Note,
                    ConstructionSiteId = projectTask.Project.ConstructionSiteId,
                    Name = projectTask.Project.Name,
                    Status = projectTask.Project.Status,
                    StartDate = projectTask.Project.StartDate,
                    EndDate = projectTask.Project.EndDate
                }
            };

            return response;
        }

        public async Task<ProjectTaskDetailsDto> GetProjectTaskDetails(Guid projectTaskId)
        {
            var projectTask = await _context.ProjectTasks
                .Include(pt => pt.Project)
                .Include(pt => pt.ProjectTaskUsers)
                    .ThenInclude(pt => pt.User)
                .FirstOrDefaultAsync(pt => pt.ProjectTaskId == projectTaskId);

            if (projectTask == null)
            {
                _logger.LogInformation("Project task not found.");
                return new ProjectTaskDetailsDto();
            }

            var mappedUsers = projectTask.ProjectTaskUsers.Select(ptu => new UserTaskDetailsDtoV2
            {
               FirstName = ptu.User.FirstName,
               LastName = ptu.User.LastName,
               Email = ptu.User.Email,
               UserId = ptu.User.UserId,
               Role = ptu.User.Role,
               IsActive = ptu.User.IsActive,
               UserTaskId = ptu.UserTaskId
            }).ToList();

            var response = new ProjectTaskDetailsDto
            {
                ProjectTaskId = projectTaskId,
                Title = projectTask.Title,
                Note = projectTask.Note,
                Status = projectTask.Status, 
                EndDate = projectTask.EndDate,
                StartDate = projectTask.StartDate,
                ProjectTaskUsers = mappedUsers,
                ProjectDetails = new ProjectDetailsDto
                {
                    Name = projectTask.Project.Name,
                    Note = projectTask.Project.Note,
                    Status = projectTask.Project.Status,
                    EndDate = projectTask.EndDate,
                    StartDate = projectTask.StartDate,
                    ProjectId = projectTask.ProjectId,
                }
            };

            return response;
        }

        public async Task<ProjectTaskSearchResponseDto> SearchProjectTasks(ProjectTaskFilterDto searchDto)
        {
            var query = _context.ProjectTasks
                .Include(pt => pt.ProjectTaskUsers)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchDto.Search))
            {
                var searchToLower = searchDto.Search.ToLower();
                query = query.Where(pt => pt.Title.Contains(searchToLower));
            }

            if (!string.IsNullOrEmpty(searchDto.Status) && Enum.TryParse<ProjectTaskStatus>(searchDto.Status, true, out var statusEnum))
            {
                query = query.Where(p => p.Status == searchDto.Status);
            }

            if (searchDto.StartFrom.HasValue)
            {
                var startUtc = DateTime.SpecifyKind(searchDto.StartFrom.Value, DateTimeKind.Utc);
                query = query.Where(p => p.StartDate >= startUtc);
            }

            if (searchDto.EndTo.HasValue)
            {
                var endUtc = DateTime.SpecifyKind(searchDto.EndTo.Value, DateTimeKind.Utc);
                query = query.Where(p => p.EndDate <= endUtc);
            }

            if (searchDto.UserId.HasValue && searchDto.UserId != Guid.Empty)
            {
                query = query.Where(pt => pt.ProjectTaskUsers.Any(tu => tu.UserId == searchDto.UserId.Value));
            }

            var total = await query.CountAsync();

            var tasks = await query
                .OrderByDescending(pt => pt.StartDate)
                .Skip((searchDto.Page - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToListAsync();

            var mappedTasks = tasks.Select(t => new ProjectTaskDetailsDto
            {
                ProjectTaskId = t.ProjectTaskId,
                Title = t.Title,
                Note = t.Note,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                Status = t.Status
            }).ToList();

            var response = new ProjectTaskSearchResponseDto
            {
                Count = total,
                Page = searchDto.Page,
                PageSize = searchDto.PageSize,
                Tasks = mappedTasks
            };

            return response;
        }

        public async Task<ProjectTaskDetailsDto> EditProjectTask(ProjectTaskEditDto dto)
        {
            var projectTask = await _context.ProjectTasks
                .Include(pt => pt.Project)
                .Include(pt => pt.ProjectTaskUsers)
                    .ThenInclude(pt => pt.User)
                .FirstOrDefaultAsync(pt => pt.ProjectTaskId == dto.ProjectTaskId);

            if (projectTask == null)
            {
                _logger.LogInformation("Project task not found.");
                return new ProjectTaskDetailsDto();
            }

            projectTask.EditProjectTask(dto.Title, dto.Note, dto.StartDate, dto.EndDate);

            await _context.SaveChangesAsync();

            var mappedUsers = projectTask.ProjectTaskUsers.Select(ptu => new UserTaskDetailsDtoV2
            {
                FirstName = ptu.User.FirstName,
                LastName = ptu.User.LastName,
                Email = ptu.User.Email,
                UserId = ptu.User.UserId,
                Role = ptu.User.Role,
                IsActive = ptu.User.IsActive,
                UserTaskId = ptu.UserTaskId
            }).ToList();

            var response = new ProjectTaskDetailsDto
            {
                ProjectTaskId = projectTask.ProjectTaskId,
                Title = projectTask.Title,
                Note = projectTask.Note,
                Status = projectTask.Status,
                EndDate = projectTask.EndDate,
                StartDate = projectTask.StartDate,
                ProjectTaskUsers = mappedUsers,
                ProjectDetails = new ProjectDetailsDto
                {
                    Name = projectTask.Project.Name,
                    Note = projectTask.Project.Note,
                    Status = projectTask.Project.Status,
                    EndDate = projectTask.EndDate,
                    StartDate = projectTask.StartDate,
                    ProjectId = projectTask.ProjectId,
                }
            };

            return response;
        }
    }
}
