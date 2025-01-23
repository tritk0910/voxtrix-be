using System.Text.Json.Serialization;

namespace Application.DTOs;

public class UserDto
{
    public required string Username { get; set; }
    public string DisplayedName { get; set; }
    public required string Email { get; set; }
    public string Token { get; set; }
}