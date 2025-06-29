using ForoUniversitario.DomainLayer.Notifications;

namespace ForoUniversitario.ApplicationLayer.Notifications;

public interface INotificationService
{
    Task<IEnumerable<NotificationDto>> GetForUserAsync(Guid userId);
    Task SendAsync(Guid receiverId, string message, TypeNotification notificationType);
    Task MarkAsReadAsync(Guid notificationId);
    Task DeleteAsync(Guid notificationId);
    Task MovePostAsync(Guid notificationId);
    Task MoveCommentAsync(Guid notificationId);
}
