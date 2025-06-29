namespace ForoUniversitario.DomainLayer.Notifications;

public interface INotificationRepository
{
    Task<IEnumerable<Notification>> GetForUserAsync(Guid userId);
    Task AddAsync(Notification notification);
    Task DeleteAsync(Guid notificationId);
    Task MovePostAsync(Guid notificationId);
    Task MoveCommentAsync(Guid notificationId);
    Task SaveChangesAsync();
}
