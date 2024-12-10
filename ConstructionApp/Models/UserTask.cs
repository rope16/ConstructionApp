namespace ConstructionApp.Models
{
    public class UserTask
    {
        public Guid UserTaskId { get; set; }
        public string Note { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid ProjectTaskId { get; set; }

        #region Relationships
        public User? User { get; set; }
        public ProjectTask? ProjectTask { get; set; }
        #endregion
        public static UserTask CreateUserTask(string note, Guid userId, Guid projectTaskId)
        {
            return new UserTask
            {
                UserTaskId = Guid.NewGuid(),
                Note = note,
                UserId = userId,
                ProjectTaskId = projectTaskId
            };
        }
    }
}
