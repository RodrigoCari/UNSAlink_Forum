namespace ForoUniversitario.DomainLayer.Notifications;

public class Notification
{
    public Guid Id { get; private set; }
    public string Message { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid ReceiverId { get; private set; }
    public TypeNotification NotificationType { get; private set; }

    private Notification() { } // EF Core

    public Notification(Guid id, string message, Guid receiverId, TypeNotification notificationType)
    {
        Id = id;
        Message = message ?? throw new ArgumentNullException(nameof(message));
        ReceiverId = receiverId;
        NotificationType = notificationType;
        CreatedAt = DateTime.UtcNow;
        IsRead = false;
    }

    public void MarkAsRead()
    {
        IsRead = true;
    }
}
