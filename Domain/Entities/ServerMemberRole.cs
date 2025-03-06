using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(ServerMemberId), nameof(RoleId), IsUnique = true)]
public class ServerMemberRole
{
    [Key]
    public string ServerMemberRoleId { get; set; } = Guid.NewGuid().ToString();
    public string ServerMemberId { get; set; }
    public ServerMember ServerMember { get; set; }
    public string RoleId { get; set; }
    public Role Role { get; set; }
}