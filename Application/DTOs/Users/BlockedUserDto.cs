namespace Application.DTOs.Users;

public class BlockedUserDto
{
    public string BlockId { get; set; }
    public string UserId { get; set; }
    public string DisplayName { get; set; }
    public string Username { get; set; }
    public string Avatar { get; set; }
}