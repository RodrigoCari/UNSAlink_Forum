using ForoUniversitario.DomainLayer.Notifications;
using Microsoft.EntityFrameworkCore;

namespace ForoUniversitario.InfrastructureLayer.Persistence;

public class NotificationRepository : INotificationRepository
{
    private readonly ForumDbContext _context;

    public NotificationRepository(ForumDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Notification>> GetForUserAsync(Guid userId)
    {
        return await _context.Notifications
                             .Where(n => n.ReceiverId == userId)
                             .ToListAsync();
    }

    public async Task AddAsync(Notification notification)
    {
        await _context.Notifications.AddAsync(notification);
    }

    public async Task DeleteAsync(Guid notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            _context.Notifications.Remove(notification);
        }
    }

    // Placeholder logic: These would contain domain-specific logic for moving posts/comments
    public async Task MovePostAsync(Guid notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            // Implement domain logic here
        }
    }

    public async Task MoveCommentAsync(Guid notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            // Implement domain logic here
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
