using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ServerMember
{
    [Key]
    public string ServerMemberId { get; set; }
    public string ServerId { get; set; }
    public Server Server { get; set; }
    public string MemberId { get; set; }
    public AppUser Member { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public bool IsBanned { get; set; }
}