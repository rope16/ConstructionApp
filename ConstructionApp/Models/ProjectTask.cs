namespace ConstructionApp.Models
{
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
    }
}
