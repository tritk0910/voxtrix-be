using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Server
{
    [Key]
    public string ServerId { get; set; } = Guid.NewGuid().ToString();
    public string ServerName { get; set; }
    public string BannerImage { get; set; }
    public string OwnerId { get; set; }
    public AppUser Owner { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<ServerMember> ServerMembers { get; set; } = [];
    public ICollection<Invite> Invites { get; set; } = [];
    public ICollection<ServerBan> Bans { get; set; } = [];
    public ICollection<Channel> Channels { get; set; } = [];
}