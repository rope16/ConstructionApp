using System.Xml.Linq;

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
        public string Title { get; set; }
        public string Note { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string TaskPhoto { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        public ICollection<UserTask> ProjectTaskUsers { get; set; } = new HashSet<UserTask>();

        #region
        public Project? Project { get; set; }
        #endregion

        public static ProjectTask CreateProjectTask(string title, string note, DateTime startDate, DateTime endDate, Guid projectId)
        {
            return new ProjectTask
            {
                ProjectTaskId = new Guid(),
                Title = title,
                Note = note,
                StartDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc),
                EndDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc),
                Status = ProjectTaskStatus.NotStarted.ToString(),
                ProjectId = projectId
            };
        }

        public void UpdateStatus(ProjectTaskStatus status)
        {
            Status = status.ToString();
        }

        public void EditProjectTask(string title, string note, DateTime startDate, DateTime endDate)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                Title = title;
            }

            if (!string.IsNullOrWhiteSpace(note))
            {
                Note = note;
            }

            if (startDate != default && startDate != DateTime.MinValue)
            {
                StartDate = startDate;
            }

            if (endDate != default && endDate != DateTime.MinValue)
            {
                EndDate = endDate;
            }
        }
    }
}
