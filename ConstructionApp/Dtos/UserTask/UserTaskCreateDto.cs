namespace ConstructionApp.Dtos.UserTask
{
    public class UserTaskCreateDto
    {
        public string Note { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid ProjectTaskId { get; set; }
    }
}
