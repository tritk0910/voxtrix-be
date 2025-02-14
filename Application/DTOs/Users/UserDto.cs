using Application.DTOs.Servers;

namespace Application.DTOs.Users;

public class UserDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string DisplayName { get; set; }
}

public class UserBasicDto : UserDto
{
    public string Avatar { get; set; }
}

public class UserDetailsDto : UserBasicDto
{
    public string Email { get; set; }
    public string Bio { get; set; }
    public string Status { get; set; }
    public string CustomStatus { get; set; }
    public DateOnly DoB { get; set; }
}