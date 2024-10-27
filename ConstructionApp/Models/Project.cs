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

        public static Project CreateProject(
            string name,
            string note,
            DateTime startDate,
            DateTime endDate,
            string status,
            Guid constructionSiteId)
        {
            return new Project
            {
                ProjectId = Guid.NewGuid(),
                Name = name,
                Note = note,
                StartDate = startDate,
                EndDate = endDate,
                Status = status,
                ConstructionSiteId = constructionSiteId
            };
        }
    }
}
