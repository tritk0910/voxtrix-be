using Domain.Entities;

namespace Application.DTOs.Servers.Roles;

public class UpdateRoleDto
{
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public string Color { get; set; }
    public RolePermission Permissions { get; set; }
}