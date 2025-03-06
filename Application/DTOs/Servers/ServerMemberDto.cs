using Application.DTOs.Servers.Roles;

namespace Application.DTOs.Servers;

public class ServerMemberDto
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string Avatar { get; set; }
    public string Status { get; set; }
    public string CustomStatus { get; set; }
    public bool IsOwner { get; set; }
    public List<RoleDto> Roles { get; set; }
}
