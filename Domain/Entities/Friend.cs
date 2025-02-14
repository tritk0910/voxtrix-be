using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Friend
{
    [Key]
    public string FriendId { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public AppUser User { get; set; }

    public string TargetId { get; set; }
    public AppUser Target { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Status Status { get; set; } = Status.Pending;
}

public enum Status
{
    Pending,
    Accepted,
    Ignored
}