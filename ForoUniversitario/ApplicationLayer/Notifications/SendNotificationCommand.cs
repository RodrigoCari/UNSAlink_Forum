using ForoUniversitario.DomainLayer.Notifications;
using System;
using System.ComponentModel.DataAnnotations;

namespace ForoUniversitario.ApplicationLayer.Notifications
{
    public class SendNotificationCommand
    {
        [Required]
        public Guid ReceiverId { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;

        [Required]
        public TypeNotification NotificationType { get; set; }
    }
}
