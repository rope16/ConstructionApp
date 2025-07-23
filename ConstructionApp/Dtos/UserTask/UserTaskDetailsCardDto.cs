using ConstructionApp.Dtos.Project;
using ConstructionApp.Dtos.ProjectTask;
using ConstructionApp.Dtos.User;

namespace ConstructionApp.Dtos.UserTask
{
    public class UserTaskDetailsCardDto
    {
        public Guid UserTaskId { get; set; }
        public string Note { get; set; }
        public ProjectTaskDetailsDto ProjectTask { get; set; }
        public UserDetailsDto User { get; set; }
    }
}
