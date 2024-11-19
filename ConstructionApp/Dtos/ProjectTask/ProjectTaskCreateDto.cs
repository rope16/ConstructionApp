namespace ConstructionApp.Dtos.ProjectTask
{
    public class ProjectTaskCreateDto
    {
        public string Note { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
    }
}
