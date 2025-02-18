namespace Application.DTOs.Notifications;

public class NotificationDto
{
    public string NotificationId { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public string Message { get; set; }
}

public class CreateNotificationDto
{
    public string UserId { get; set; }
    public string Message { get; set; }
}

public class UpdateNotificationDto
{
    public string NotificationId { get; set; }
    public bool IsRead { get; set; }
}