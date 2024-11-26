using ConstructionApp.Dtos.ProjectTask;

namespace ConstructionApp.Interfaces.ProjectTasksInterface
{
    public interface IProjectTaskInterface
    {
        Task<ProjectTaskDetailsDto> CreateProjectTask(ProjectTaskCreateDto dto);
        Task<ProjectTaskDetailsDto> UploadProjectTaskPhoto(IFormFile image, Guid projectTaskId);

        Task<List<ProjectTaskDetailsDto>> GetAllProjectTasks();

        Task<bool> DeleteProjectTask(Guid projectId);
    }
}
