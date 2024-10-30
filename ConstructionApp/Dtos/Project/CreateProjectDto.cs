namespace ConstructionApp.Dtos.Project
{
    public class CreateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ConstructionSiteId { get; set; }
    }
}
