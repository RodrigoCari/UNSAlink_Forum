using ForoUniversitario.DomainLayer.Notifications;

namespace ForoUniversitario.ApplicationLayer.Notifications;

public class SendNotificationCommand
{
    public Guid ReceiverId { get; set; }
    public string Message { get; set; } = string.Empty;
    public TypeNotification NotificationType { get; set; }
}
