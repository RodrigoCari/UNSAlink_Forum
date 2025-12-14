using ForoUniversitario.DomainLayer.Notifications;
using ForoUniversitario.InfrastructureLayer.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ForoUniversitario.Tests.InfrastructureLayer.Persistence;

public class NotificationRepositoryTests : IDisposable
{
    private readonly ForumDbContext _context;
    private readonly NotificationRepository _repository;

    public NotificationRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ForumDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ForumDbContext(options);
        _repository = new NotificationRepository(_context);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    [Fact]
    public async Task AddAsync_WithValidNotification_ShouldAddToDatabase()
    {
        // Arrange
        var notification = new Notification(Guid.NewGuid(), "Test notification", Guid.NewGuid(), TypeNotification.NewComment);

        // Act
        await _repository.AddAsync(notification);
        await _repository.SaveChangesAsync();

        // Assert
        var savedNotification = await _context.Notifications.FindAsync(notification.Id);
        Assert.NotNull(savedNotification);
        Assert.Equal(notification.Message, savedNotification.Message);
        Assert.Equal(notification.ReceiverId, savedNotification.ReceiverId);
    }

    [Fact]
    public async Task GetForUserAsync_WithUserNotifications_ShouldReturnNotifications()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notification1 = new Notification(Guid.NewGuid(), "Notification 1", userId, TypeNotification.NewComment);
        var notification2 = new Notification(Guid.NewGuid(), "Notification 2", userId, TypeNotification.NewPost);

        await _context.Notifications.AddRangeAsync(notification1, notification2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetForUserAsync(userId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, n => n.Message == "Notification 1");
        Assert.Contains(result, n => n.Message == "Notification 2");
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingNotification_ShouldReturnNotification()
    {
        // Arrange
        var notification = new Notification(Guid.NewGuid(), "Test notification", Guid.NewGuid(), TypeNotification.NewComment);
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(notification.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(notification.Id, result.Id);
        Assert.Equal(notification.Message, result.Message);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingNotification_ShouldRemoveNotification()
    {
        // Arrange
        var notification = new Notification(Guid.NewGuid(), "Test notification", Guid.NewGuid(), TypeNotification.NewComment);
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(notification.Id);
        await _repository.SaveChangesAsync();

        // Assert
        var deletedNotification = await _context.Notifications.FindAsync(notification.Id);
        Assert.Null(deletedNotification);
    }
}
