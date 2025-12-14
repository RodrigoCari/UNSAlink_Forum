namespace ForoUniversitario.Tests.DomainLayer.DomainServices;

public class PostDomainServiceTests
{
    private readonly PostDomainService _service;

    public PostDomainServiceTests()
    {
        _service = new PostDomainService();
    }

    [Fact]
    public async Task CanSharePostToGroupAsync_WhenDifferentGroup_ReturnsTrue()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var originalGroupId = Guid.NewGuid();
        var targetGroupId = Guid.NewGuid();

        var post = new Post(postId, "Test Post", new PostContent("Content"),
            Guid.NewGuid(), originalGroupId, TypePost.Discussion);

        // Act
        var result = await _service.CanSharePostToGroupAsync(post, targetGroupId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CanSharePostToGroupAsync_WhenSameGroup_ReturnsFalse()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var groupId = Guid.NewGuid();

        var post = new Post(postId, "Test Post", new PostContent("Content"),
            Guid.NewGuid(), groupId, TypePost.Discussion);

        // Act
        var result = await _service.CanSharePostToGroupAsync(post, groupId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CanSharePostToGroupAsync_WhenPostIsNull_ThrowsException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() =>
            _service.CanSharePostToGroupAsync(null!, Guid.NewGuid()));
    }
}
