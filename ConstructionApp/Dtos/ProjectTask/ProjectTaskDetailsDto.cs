using ConstructionApp.Dtos.Project;
using ConstructionApp.Dtos.User;
using ConstructionApp.Dtos.UserTask;

namespace ConstructionApp.Dtos.ProjectTask
{
    public class ProjectTaskDetailsDto
    {
        public Guid ProjectTaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string TaskPhoto { get; set; } = string.Empty;
        public ProjectDetailsDto? ProjectDetails { get; set; }
        public List<UserTaskDetailsDtoV2> ProjectTaskUsers { get; set; }
    }
}
