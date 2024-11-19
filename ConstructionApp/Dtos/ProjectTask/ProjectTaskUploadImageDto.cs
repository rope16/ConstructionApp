namespace ConstructionApp.Dtos.ProjectTask
{
    public class ProjectTaskUploadImageDto
    {
        public Guid ProjectTaskId { get; set; }
        public IFormFile Image { get; set; }
    }
}
