using ForoUniversitario.DomainLayer.Notifications;
using Microsoft.EntityFrameworkCore;

namespace ForoUniversitario.InfrastructureLayer.Persistence;

public class NotificationRepository : INotificationRepository
{
    private readonly ForumDbContext _context;
    public NotificationRepository(ForumDbContext context) => _context = context;

    public async Task<IEnumerable<Notification>> GetForUserAsync(Guid userId) =>
        await _context.Notifications
                      .Where(n => n.ReceiverId == userId)
                      .ToListAsync();

    public async IAsyncEnumerable<Notification> StreamForUserAsync(Guid userId)
    {
        await foreach (var n in _context.Notifications
                                        .Where(n => n.ReceiverId == userId)
                                        .AsAsyncEnumerable())
            yield return n;
    }

    public async Task AddAsync(Notification notification) =>
        await _context.Notifications.AddAsync(notification);

    public async Task DeleteAsync(Guid notificationId)
    {
        var notif = await _context.Notifications.FindAsync(notificationId);
        if (notif is not null) _context.Notifications.Remove(notif);
    }

    public Task MovePostAsync(Guid notificationId) => Task.CompletedTask;
    public Task MoveCommentAsync(Guid notificationId) => Task.CompletedTask;

    public Task SaveChangesAsync() => _context.SaveChangesAsync();

    public Task<Notification?> GetByIdAsync(Guid id) =>
        _context.Notifications.FindAsync(id).AsTask();
}
