namespace Application.DTOs.Users;

public class UserDto
{
    public string Id { get; set; }
    public required string Username { get; set; }
    public string DisplayName { get; set; }
    public required string Email { get; set; }
}