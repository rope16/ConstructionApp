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
        public async Task<UserTaskDetailsDto> CreateUserTask(UserTaskCreateDto dto)
        {
            var userTask=await _context.UserTasks.FirstOrDefaultAsync(u=>u.UserId==dto.UserId && u.ProjectTaskId==dto.ProjectTaskId);
            if(userTask!=null)
            {
                _logger.LogInformation("User is already assigned to this task");
                return new UserTaskDetailsDto();
            }
            var newUserTask=UserTask.CreateUserTask(dto.Note, dto.UserId, dto.ProjectTaskId);
            var response=new UserTaskDetailsDto
            {
                UserTaskId = newUserTask.UserTaskId,
                Note = newUserTask.Note,
                UserId = newUserTask.UserId,
                ProjectTaskId = newUserTask.ProjectTaskId,
            };
            _context.Add(newUserTask);
            await _context.SaveChangesAsync();
            return response;

        }
        public async Task<bool> DeleteUserTask (Guid userTaskId)
        {
            var userTask = await _context.UserTasks.FirstOrDefaultAsync(u=>u.UserTaskId == userTaskId);
            if(userTask==null)
            {
                _logger.LogInformation("Task with provided Id was not found.");
                return false;
            }
            _context.Remove(userTask);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
