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
        Task<ProjectTaskDetailsDto> UpdateProjectTaskStatus(Guid projectTaskId, string status);
        Task<ProjectTaskDetailsDto> GetProjectTaskDetails(Guid projectTaskId);
        Task<ProjectTaskSearchResponseDto> SearchProjectTasks(ProjectTaskFilterDto searchDto);
        Task<ProjectTaskDetailsDto> EditProjectTask(ProjectTaskEditDto dto);
    }
}
