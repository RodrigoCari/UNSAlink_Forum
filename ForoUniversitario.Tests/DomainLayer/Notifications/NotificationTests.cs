using ForoUniversitario.DomainLayer.Notifications;
using Xunit;

namespace ForoUniversitario.Tests.DomainLayer.Notifications;

public class NotificationTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateNotification()
    {
        // Arrange
        var id = Guid.NewGuid();
        var message = "Test notification";
        var receiverId = Guid.NewGuid();
        var type = TypeNotification.NewComment;

        // Act
        var notification = new Notification(id, message, receiverId, type);

        // Assert
        Assert.Equal(id, notification.Id);
        Assert.Equal(message, notification.Message);
        Assert.Equal(receiverId, notification.ReceiverId);
        Assert.Equal(type, notification.NotificationType);
        Assert.False(notification.IsRead);
        Assert.InRange(notification.CreatedAt, DateTime.UtcNow.AddSeconds(-5), DateTime.UtcNow.AddSeconds(5));
    }

    [Theory]
    [InlineData("")]        // Cambiar de null a string vacío
    [InlineData("   ")]
    public void Constructor_WithInvalidMessage_ShouldThrowDomainException(string invalidMessage)
    {
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            new Notification(Guid.NewGuid(), invalidMessage, Guid.NewGuid(), TypeNotification.NewComment));

        Assert.Equal("Notification message cannot be empty.", exception.Message);
    }

    [Fact]
    public void MarkAsRead_ShouldSetIsReadToTrue()
    {
        // Arrange
        var notification = new Notification(Guid.NewGuid(), "Test", Guid.NewGuid(), TypeNotification.NewComment);

        // Act
        notification.MarkAsRead();

        // Assert
        Assert.True(notification.IsRead);
    }
}
