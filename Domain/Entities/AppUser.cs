using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(UserName), nameof(Email), IsUnique = true)]
public class AppUser : IdentityUser
{
    public string DisplayName { get; set; } = "";
    public string Avatar { get; set; }
    public string CustomStatus { get; set; } = "";
    public UserStatus Status { get; set; } = UserStatus.Offline;
    public string Bio { get; set; } = "";
    public DateOnly DoB { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<ServerMember> ServerMembers { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
    public ICollection<Message> SentMessages { get; set; } = [];  // Messages the user sent
    public ICollection<Message> ReceivedMessages { get; set; } = [];  // Messages the user received
    public ICollection<Reaction> Reactions { get; set; } = [];
    public ICollection<Invite> Invites { get; set; } = [];
    public ICollection<Server> OwnedServers { get; set; } = [];
    public ICollection<ServerBan> ServerBans { get; set; } = [];
    public ICollection<Role> ServerRoles { get; set; } = [];
    public ICollection<Friend> Friends { get; set; } = [];
    public ICollection<UserBlock> BlockedUsers { get; set; } = [];
}

public enum UserStatus
{
    Online,
    Offline,
    Idle,
    DoNotDisturb
}