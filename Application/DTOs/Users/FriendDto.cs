namespace Application.DTOs.Users
{
    public class FriendDto
    {
        public string RequestId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
    }

    public class FriendResponseDto
    {
        public string RequestId { get; set; }
        public string Status { get; set; }
    }
}
