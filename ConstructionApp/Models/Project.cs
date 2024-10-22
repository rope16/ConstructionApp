namespace ConstructionApp.Models
{
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
        #endregion
    }
}
