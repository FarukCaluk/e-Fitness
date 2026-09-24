using eFitness.Domain.Entities;
using eFitness.Domain.Enums;

namespace eFitness.Application.Notifications;

public record NotificationDto(int Id, string Title, string Message, NotificationType Type, bool IsRead, DateTime CreatedAt)
{
    public static NotificationDto FromEntity(Notification notification) => new(
        notification.Id,
        notification.Title,
        notification.Message,
        notification.Type,
        notification.IsRead,
        notification.CreatedAt);
}
