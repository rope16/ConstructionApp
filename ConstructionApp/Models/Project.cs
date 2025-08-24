namespace ConstructionApp.Models
{
    public enum ProjectStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Cancelled
    }
    public class Project
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid ConstructionSiteId { get; set; }

        #region #Relationships
        public ConstructionSite? ConstructionSite { get; set; }

        public ICollection<ProjectTask> Tasks { get; set; } = new HashSet<ProjectTask>();
        #endregion

        public static Project CreateProject(
            string name,
            string note,
            DateTime startDate,
            DateTime endDate,
            Guid constructionSiteId)
        {
            return new Project
            {
                ProjectId = Guid.NewGuid(),
                Name = name,
                Note = note,
                StartDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc),
                EndDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc),
                Status = ProjectStatus.NotStarted.ToString(),
                ConstructionSiteId = constructionSiteId
            };
        }

        public void UpdateProjectStatus(string status)
        {
            Status = status;
        }

        public void EditProject(string name, string note, DateTime startDate, DateTime endDate)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Name = name;
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
