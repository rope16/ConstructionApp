namespace ConstructionApp.Dtos.UserTask
{
    public class UserTaskDetailsDto
    {
        public Guid UserTaskId { get; set; }
        public string Note { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid ProjectTaskId { get; set; }
    }
}
