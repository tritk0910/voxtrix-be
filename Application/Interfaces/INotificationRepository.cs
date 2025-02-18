using Application.Core;
using Application.DTOs.Notifications;

namespace Application.Interfaces;

public interface INotificationRepository
{
    Task<Result<NotificationDto>> CreateNotificationAsync(CreateNotificationDto createNotificationDto);
    Task<Result<bool>> DeleteNotificationAsync(string notificationId);
    Task<Result<List<NotificationDto>>> GetNotificationsByUserIdAsync(string userId);
    Task<Result<NotificationDto>> UpdateNotificationAsync(UpdateNotificationDto updateNotificationDto);
}