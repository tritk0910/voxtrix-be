using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Notification
{
    [Key]
    public string NotificationId { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }
}

public class FriendRequestNotification : Notification
{
    public string RequesterId { get; set; }
    public AppUser Requester { get; set; }
}