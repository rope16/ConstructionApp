using ConstructionApp.Dtos.User;
using ConstructionApp.Dtos.UserTask;
using ConstructionApp.Interfaces.UserTaskInterfaces;
using ConstructionApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionApp.Services.UserTaskService
{
    public class UserTaskService: UserTaskServices
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserTaskService> _logger;

        public UserTaskService(ApplicationDbContext context, ILogger<UserTaskService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<UserTaskDetailsDtoV2> CreateUserTask(UserTaskCreateDto dto)
        {
            var userTask = await _context.UserTasks.FirstOrDefaultAsync(u => u.UserId == dto.UserId && u.ProjectTaskId == dto.ProjectTaskId);

            if (userTask != null)
            {
                _logger.LogInformation("User is already assigned to this task");
                return new UserTaskDetailsDtoV2();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == dto.UserId);

            if (user == null)
            {
                _logger.LogInformation($"User with id {dto.UserId} not found.");
                return new UserTaskDetailsDtoV2();
            }

            var projectTask = await _context.ProjectTasks.FirstOrDefaultAsync(p => p.ProjectTaskId == dto.ProjectTaskId);

            if (projectTask == null)
            {
                _logger.LogInformation($"Project task with id {dto.ProjectTaskId} not found.");
                return new UserTaskDetailsDtoV2();
            }

            var newUserTask = UserTask.CreateUserTask(dto.Note, dto.UserId, dto.ProjectTaskId);

            var response = new UserTaskDetailsDtoV2
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserId = user.UserId,
                Role = user.Role,
                IsActive = user.IsActive,
                UserTaskId = newUserTask.UserTaskId
            };

            _context.Add(newUserTask);

            await _context.SaveChangesAsync();

            return response;
        }
        public async Task<bool> DeleteUserTask (Guid userTaskId)
        {
            var userTask = await _context.UserTasks.FirstOrDefaultAsync(u => u.UserTaskId == userTaskId);

            if(userTask==null)
            {
                _logger.LogInformation("Task with provided Id was not found.");
                return false;
            }

            _context.Remove(userTask);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<UserTaskDetailsCardDto>> GetUserTasks(Guid userId, int pageNumber, int pageSize)
        {
            var userTasks = await _context.UserTasks
                .Include(ut => ut.ProjectTask)
                .ThenInclude(pt => pt.Project)
                .Include(ut => ut.User)
                .Where(ut => ut.UserId == userId)
                .OrderBy(ut => ut.ProjectTask.EndDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (userTasks.Count == 0)
            {
                _logger.LogInformation("No tasks found for the user.");
                return new List<UserTaskDetailsCardDto>();
            }

            return userTasks.Select(ut => new UserTaskDetailsCardDto
            {
                UserTaskId = ut.UserTaskId,
                Note = ut.Note,
                ProjectTask = new Dtos.ProjectTask.ProjectTaskDetailsDto
                {
                    ProjectTaskId = ut.ProjectTask.ProjectTaskId,
                    Title = ut.ProjectTask.Title,
                    Note = ut.ProjectTask.Note,
                    Status = ut.ProjectTask.Status,
                    StartDate = ut.ProjectTask.StartDate.Date,
                    EndDate = ut.ProjectTask.EndDate.Date,
                    ProjectDetails = new Dtos.Project.ProjectDetailsDto
                    {
                        ProjectId = ut.ProjectTask.Project.ProjectId,
                        Name = ut.ProjectTask.Project.Name,
                        Note = ut.ProjectTask.Project.Note,
                        Status = ut.ProjectTask.Project.Status,
                        StartDate = ut.ProjectTask.Project.StartDate.Date,
                        EndDate = ut.ProjectTask.Project.EndDate.Date,
                        ConstructionSiteId = ut.ProjectTask.Project.ConstructionSiteId
                    }
                },
                User = new Dtos.User.UserDetailsDto
                {
                    UserId = ut.User.UserId,
                    FirstName = ut.User.FirstName,
                    LastName = ut.User.LastName,
                    Email = ut.User.Email
                }
            }).ToList();
        }
    }
}
