using ForoUniversitario.DomainLayer.Notifications;

namespace ForoUniversitario.ApplicationLayer.Notifications;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;

    public NotificationService(INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<NotificationDto>> GetForUserAsync(Guid userId)
    {
        var notifications = await _repository.GetForUserAsync(userId);
        return notifications.Select(n => new NotificationDto
        {
            Id = n.Id,
            Message = n.Message,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt,
            NotificationType = n.NotificationType
        });
    }

    public async Task SendAsync(Guid receiverId, string message, TypeNotification notificationType)
    {
        var notification = new Notification(Guid.NewGuid(), message, receiverId, notificationType);
        await _repository.AddAsync(notification);
        await _repository.SaveChangesAsync();
    }

    public async Task MarkAsReadAsync(Guid notificationId)
    {
        var notifications = await _repository.GetForUserAsync(Guid.Empty); // Could be optimized with a GetById
        var notification = notifications.FirstOrDefault(n => n.Id == notificationId);
        if (notification != null)
        {
            notification.MarkAsRead();
            await _repository.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Guid notificationId)
    {
        await _repository.DeleteAsync(notificationId);
        await _repository.SaveChangesAsync();
    }

    public async Task MovePostAsync(Guid notificationId)
    {
        await _repository.MovePostAsync(notificationId);
        await _repository.SaveChangesAsync();
    }

    public async Task MoveCommentAsync(Guid notificationId)
    {
        await _repository.MoveCommentAsync(notificationId);
        await _repository.SaveChangesAsync();
    }
}
