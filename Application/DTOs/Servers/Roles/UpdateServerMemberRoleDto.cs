namespace Application.DTOs.Servers.Roles;

public class UpdateOrDeleteServerMemberRoleDto
{
    public string ServerId { get; set; }
    public string UserId { get; set; }
    public string RoleId { get; set; }
}