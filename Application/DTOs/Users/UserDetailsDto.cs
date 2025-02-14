using Domain.Entities;

namespace Application.DTOs.Users;

public class UserDetailsDto
{
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string Bio { get; set; }
    public string Status { get; set; }
    public string CustomStatus { get; set; }
    public DateOnly DoB { get; set; }
}