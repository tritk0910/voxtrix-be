namespace Application.DTOs.Accounts;

public class UserDetailDto
{
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public bool IsAdmin { get; set; }
    public DateOnly DoB { get; set; }
}