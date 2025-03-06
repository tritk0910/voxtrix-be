using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.DTOs.Servers.Roles;

public class CreateRoleDto
{
    public string ServerId { get; set; }
    public string RoleName { get; set; }
    public string Color { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RolePermission Permissions { get; set; }
}