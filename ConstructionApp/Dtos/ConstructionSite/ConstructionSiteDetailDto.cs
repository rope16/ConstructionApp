namespace ConstructionApp.Dtos.ConstructionSite
{
    public class ConstructionSiteDetailDto
    {
        public Guid ConstructionSiteId { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Contractor { get; set; } = string.Empty;
        public string Investor { get; set; } = string.Empty;
    }
}
