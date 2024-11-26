using ConstructionApp.Dtos.Project;

namespace ConstructionApp.Interfaces.ProjectInterfaces
{
    public interface IProjectService
    {
        Task<List<ProjectDetailsDto>> GetAllProjects();
        Task<ProjectDetailsDto> CreateProject(CreateProjectDto dto);
        Task<bool> DeleteProject(Guid projectId);
        Task<ProjectDetailsDto> UpdateProjectStatus(Guid projectId, string status);

        Task<ProjectDetailsDto> GetProjectWithTasks (Guid projectId);
    }
}
