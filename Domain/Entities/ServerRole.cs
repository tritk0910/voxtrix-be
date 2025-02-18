using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(UserId), nameof(RoleId), nameof(ServerId), IsUnique = true)]
public class ServerRole
{
    [Key]
    public string ServerRoleId { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public string RoleId { get; set; }
    public Role Role { get; set; }
    public string ServerId { get; set; }
    public Server Server { get; set; }
}