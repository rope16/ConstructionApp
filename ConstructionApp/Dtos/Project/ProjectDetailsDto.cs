using ConstructionApp.Dtos.ConstructionSite;

namespace ConstructionApp.Dtos.Project
{
    public class ProjectDetailsDto
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid? ConstructionSiteId { get; set; }
        public ConstructionSiteDetailDto? ConstructionSiteDetails { get; set; }
    }
}
