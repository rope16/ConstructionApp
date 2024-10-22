namespace ConstructionApp.Models
{
    public class ConstructionSite
    {
        public Guid ConstructionSiteId { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Contractor { get; set; } = string.Empty;
        public string Investor { get; set; } = string.Empty;

        public static ConstructionSite CreateConstructionSite (string address, string contractor, string investor)
        {
            return new ConstructionSite
            {
                ConstructionSiteId = Guid.NewGuid(),
                Address = address,
                Contractor = contractor,
                Investor = investor
            };
        }
    }
}
