namespace Application.DTOs.Users;

public class FriendRequestDto
{
    public string RequestId { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public string Avatar { get; set; }
}