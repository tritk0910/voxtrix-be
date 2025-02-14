using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class FriendshipRelation
{
    [Key]
    public string FriendshipRelationId { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public AppUser User { get; set; }

    public string FriendId { get; set; }
    public AppUser Friend { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Status Status { get; set; } = Status.Pending;
}

public enum Status
{
    Pending,
    Accepted,
    Ignored
}