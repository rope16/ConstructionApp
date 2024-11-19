using ConstructionApp.Dtos.ProjectTask;

namespace ConstructionApp.Interfaces.ProjectTasksInterface
{
    public interface IProjectTaskInterface
    {
        Task<ProjectTaskDetailsDto> CreateProjectTask(ProjectTaskCreateDto dto);
    }
}
