namespace ConstructionApp.Dtos.ConstructionSite
{
    public class ConstructionSiteSearchResponseDto
    {
        public int Count { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<ConstructionSiteDetailDto> Sites { get; set; }
    }
}
