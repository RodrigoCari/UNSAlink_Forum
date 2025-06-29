using ForoUniversitario.DomainLayer.Notifications;

namespace ForoUniversitario.ApplicationLayer.Notifications;

public class NotificationDto
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public TypeNotification NotificationType { get; set; }
}
