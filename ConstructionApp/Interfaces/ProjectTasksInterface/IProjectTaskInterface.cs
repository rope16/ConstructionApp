using ConstructionApp.Dtos.ProjectTask;
using ConstructionApp.Dtos.User;

namespace ConstructionApp.Interfaces.ProjectTasksInterface
{
    public interface IProjectTaskInterface
    {
        Task<ProjectTaskDetailsDto> CreateProjectTask(ProjectTaskCreateDto dto);
        Task<ProjectTaskDetailsDto> UploadProjectTaskPhoto(IFormFile image, Guid projectTaskId);
        Task<List<ProjectTaskDetailsDto>> GetAllProjectTasks();
        Task<bool> DeleteProjectTask(Guid projectId);
        Task<List<UserDetailsDto>> GetProjectTaskUsers(Guid projectTaskId);
    }
}
