namespace Application.DTOs.Accounts;

public class UserDto
{
    public required string Username { get; set; }
    public string DisplayName { get; set; }
    public required string Email { get; set; }
    public string Token { get; set; }
}