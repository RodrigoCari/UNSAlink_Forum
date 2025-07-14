namespace ForoUniversitario.DomainLayer.Notifications;

public class Notification
{
    public Guid Id { get; private set; }
    public string Message { get; private set; } = default!;
    public bool IsRead { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid ReceiverId { get; private set; }
    public TypeNotification NotificationType { get; private set; }

    private Notification() { }   // EF Core

    public Notification(Guid id, string message, Guid receiverId, TypeNotification type)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new DomainException("Notification message cannot be empty.");

        Id = id;
        Message = message;
        ReceiverId = receiverId;
        NotificationType = type;
        CreatedAt = DateTime.UtcNow;
        IsRead = false;
    }

    public void MarkAsRead() => IsRead = true;
}