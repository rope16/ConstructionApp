﻿namespace ConstructionApp.Dtos.UserTask
{
    public class UserTaskDetailsDtoV2
    {
        public Guid UserId { get; set; }
        public Guid UserTaskId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
