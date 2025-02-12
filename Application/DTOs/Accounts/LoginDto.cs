namespace Application.DTOs.Accounts;

public class LoginDto
{
    public required string UsernameOrEmail { get; set; }
    public required string Password { get; set; }
}