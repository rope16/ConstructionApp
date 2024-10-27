using ConstructionApp.Dtos.Project;

namespace ConstructionApp.Interfaces.ProjectInterfaces
{
    public interface IProjectService
    {
        Task<ProjectDetailsDto> CreateProject(CreateProjectDto dto);
    }
}
