using ConstructionApp.Dtos.Project;

namespace ConstructionApp.Dtos.ProjectTask
{
    public class ProjectTaskDetailsDto
    {
        public Guid ProjectTaskId { get; set; }
        public string Note { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string TaskPhoto { get; set; } = string.Empty;
        public ProjectDetailsDto Project { get; set; }
    }
}
