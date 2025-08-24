namespace ConstructionApp.Dtos.User
{
    public class UserSearchResponseDto
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<UserDetailsDto> Users { get; set; }
    }
}
