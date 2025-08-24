namespace ConstructionApp.Dtos.ProjectTask
{
    public class ProjectTaskEditDto
    {
        public Guid ProjectTaskId { get; set; }
        public string Title { get; set; }
        public string Note { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
