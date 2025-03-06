using Domain.Entities;

namespace Application.DTOs.Servers.Roles;

public class RoleDto
{
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public string Color { get; set; }
}

public class RoleDetailsDto : RoleDto
{
    public int MemberCount { get; set; }
    public int Position { get; set; }
    public RolePermission Permissions { get; set; }
}