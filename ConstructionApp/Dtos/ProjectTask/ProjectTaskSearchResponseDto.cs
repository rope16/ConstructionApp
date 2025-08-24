namespace ConstructionApp.Dtos.ProjectTask
{
    public class ProjectTaskSearchResponseDto
    {
        public int Count { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<ProjectTaskDetailsDto> Tasks { get; set; }
    }
}
