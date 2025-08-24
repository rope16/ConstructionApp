using ConstructionApp.Dtos.User;
using ConstructionApp.Dtos.UserTask;

namespace ConstructionApp.Interfaces.UserTaskInterfaces
{
    public interface UserTaskServices
    {
        Task<UserTaskDetailsDtoV2> CreateUserTask(UserTaskCreateDto dto);

        Task<bool> DeleteUserTask(Guid userTaskId);

        Task<List<UserTaskDetailsCardDto>> GetUserTasks(Guid userId, int pageNumber, int pageSize);
    }

}
