namespace ConstructionApp.Models
{
    public enum ProjectTaskStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Cancelled
    }
    public class ProjectTask
    {
        public Guid ProjectTaskId { get; set; }
        public string Note { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string TaskPhoto { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        #region
        public Project? Project { get; set; }
        #endregion

        public static ProjectTask CreateProjectTask(string note, DateTime startDate, DateTime endDate, Guid projectId)
        {
            return new ProjectTask
            {
                ProjectTaskId = new Guid(),
                Note = note,
                StartDate = startDate,
                EndDate = endDate,
                Status = ProjectTaskStatus.NotStarted.ToString(),
                ProjectId = projectId
            };
        }
    }
}
