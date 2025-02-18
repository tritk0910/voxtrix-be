using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(ServerId), nameof(MemberId), IsUnique = true)]
public class ServerMember
{
    [Key]
    public string ServerMemberId { get; set; }
    public bool IsOwner { get; set; }
    public string ServerId { get; set; }
    public Server Server { get; set; }
    public string MemberId { get; set; }
    public AppUser Member { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public bool IsBanned { get; set; }
    public List<ServerRole> UserRoles { get; set; }
}