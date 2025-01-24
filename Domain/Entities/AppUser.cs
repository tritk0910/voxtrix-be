using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; } = "";
    public string Avatar { get; set; }
    public UserStatus Status { get; set; }
    public string Bio { get; set; }
    public DateOnly DoB { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<ServerMember> ServerMembers { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
    public ICollection<Reaction> Reactions { get; set; } = [];
    public ICollection<Invite> Invites { get; set; } = [];
    public ICollection<Server> OwnedServers { get; set; } = [];
    public ICollection<ServerBan> ServerBans { get; set; } = [];
}

public enum UserStatus
{
    Online,
    Offline,
    Idle,
    DoNotDisturb
}