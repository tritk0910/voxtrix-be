namespace Application.DTOs;

public class UserDetailDto
{
    public string Username { get; set; }
    public string DisplayedName { get; set; }
    public string Email { get; set; }
    public bool IsAdmin { get; set; }
    public DateOnly DoB { get; set; }
}