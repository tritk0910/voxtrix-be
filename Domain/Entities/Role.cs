using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Role
{
    [Key]
    public string RoleId { get; set; } = Guid.NewGuid().ToString();
    [StringLength(100)]
    public string RoleName { get; set; }
    // Store permissions as a long (bitwise representation)
    public long Permissions { get; set; }
    [StringLength(7)]
    public string Color { get; set; }
    public int Position { get; set; }
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<ServerRole> ServerRoles { get; set; } = [];
}

public static class RolePermissionHelper
{
    // Convert enum to long for storage
    public static long ToLong(this RolePermission permissions)
    {
        return (long)permissions;
    }

    // Convert long back to RolePermission enum
    public static RolePermission FromLong(long value)
    {
        return (RolePermission)value;
    }
}

[Flags]
public enum RolePermission
{
    None = 0,
    CreateInstantInvite = 1 << 0,
    KickMembers = 1 << 1,
    BanMembers = 1 << 2,
    Administrator = 1 << 3,
    ManageChannels = 1 << 4,
    ManageGuild = 1 << 5,
    AddReactions = 1 << 6,
    ViewAuditLog = 1 << 7,
    PrioritySpeaker = 1 << 8,
    Stream = 1 << 9,
    ViewChannel = 1 << 10,
    SendMessages = 1 << 11,
    SendTTSMessages = 1 << 12,
    ManageMessages = 1 << 13,
    EmbedLinks = 1 << 14,
    AttachFiles = 1 << 15,
    ReadMessageHistory = 1 << 16,
    MentionEveryone = 1 << 17,
    UseExternalEmojis = 1 << 18,
    Connect = 1 << 20,
    Speak = 1 << 21,
    MuteMembers = 1 << 22,
    DeafenMembers = 1 << 23,
    MoveMembers = 1 << 24,
    UseVAD = 1 << 25,
    ChangeNickname = 1 << 26,
    ManageNicknames = 1 << 27,
    ManageRoles = 1 << 28,
    ManageWebhooks = 1 << 29,
    ManageEmojisAndStickers = 1 << 30,
    RequestToSpeak = 1 << 32,
    ManageThreads = 1 << 34,
    CreatePublicThreads = 1 << 35,
    CreatePrivateThreads = 1 << 36,
    UseExternalStickers = 1 << 37,
    SendMessagesInThreads = 1 << 38
}
