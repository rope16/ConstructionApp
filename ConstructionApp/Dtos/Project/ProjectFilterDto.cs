namespace ConstructionApp.Dtos.Project
{
    public class ProjectFilterDto
    {
        public string? Search { get; set; }
        public string? Status { get; set; }
        public DateTime? StartFrom { get; set; }
        public DateTime? EndTo { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid? UserId { get; set; }
    }
}
