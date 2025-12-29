using ForoUniversitario.DomainLayer.Factories;
using Microsoft.Extensions.Logging;

namespace ForoUniversitario.Tests.ApplicationLayer.Posts;

public class CommentServiceTests
{
    private readonly Mock<ICommentRepository> _mockCommentRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPostRepository> _mockPostRepository;
    private readonly Mock<INotificationService> _mockNotificationService;
    private readonly Mock<ICommentFactory> _mockCommentFactory;
    private readonly Mock<ILogger<CommentService>> _mockLogger;
    private readonly CommentService _commentService;

    public CommentServiceTests()
    {
        _mockCommentRepository = new Mock<ICommentRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPostRepository = new Mock<IPostRepository>();
        _mockNotificationService = new Mock<INotificationService>();
        _mockCommentFactory = new Mock<ICommentFactory>();
        _mockLogger = new Mock<ILogger<CommentService>>();

        _commentService = new CommentService(
            _mockCommentRepository.Object,
            _mockUserRepository.Object,
            _mockPostRepository.Object,
            _mockNotificationService.Object,
            _mockCommentFactory.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task AddCommentAsync_WithValidData_ShouldAddCommentAndSendNotification()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var postAuthorId = Guid.NewGuid();
        var postId = Guid.NewGuid();
        var user = new User(userId, "Commenter", "commenter@email.com", Role.Student, "hash");
        var post = new Post(postId, "Test Post", new PostContent("Content"), postAuthorId, Guid.NewGuid(), TypePost.Discussion);

        var command = new AddCommentCommand
        {
            Content = "Great post!",
            PostId = postId
        };

        var comment = new Comment(Guid.NewGuid(), command.Content, user.Name, postId);

        _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);
        _mockPostRepository.Setup(x => x.GetByIdAsync(postId)).ReturnsAsync(post);
        _mockCommentRepository.Setup(x => x.AddAsync(It.IsAny<Comment>(), postId)).Returns(Task.CompletedTask);
        _mockCommentFactory.Setup(x => x.Create(command.Content, user.Name, postId)).Returns(comment);
        _mockCommentRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);
        _mockNotificationService.Setup(x => x.SendAsync(postAuthorId, It.IsAny<string>(), TypeNotification.NewComment))
            .Returns(Task.CompletedTask);

        // Act
        await _commentService.AddCommentAsync(command, userId);

        // Assert
        _mockCommentFactory.Verify(x => x.Create(command.Content, user.Name, postId), Times.Once);
        _mockCommentRepository.Verify(x => x.AddAsync(It.Is<Comment>(c =>
            c.Content == command.Content &&
            c.Author == user.Name), postId), Times.Once);
        _mockCommentRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        _mockNotificationService.Verify(x => x.SendAsync(postAuthorId,
            It.Is<string>(m => m.Contains(user.Name) && m.Contains(post.Title)),
            TypeNotification.NewComment), Times.Once);
    }

    [Fact]
    public async Task AddCommentAsync_WhenCommentingOwnPost_ShouldNotSendNotification()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var postId = Guid.NewGuid();
        var user = new User(userId, "Author", "author@email.com", Role.Student, "hash");
        var post = new Post(postId, "My Post", new PostContent("Content"), userId, Guid.NewGuid(), TypePost.Discussion);

        var command = new AddCommentCommand
        {
            Content = "My own comment",
            PostId = postId
        };

        var comment = new Comment(Guid.NewGuid(), command.Content, user.Name, postId);

        _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);
        _mockPostRepository.Setup(x => x.GetByIdAsync(postId)).ReturnsAsync(post);
        _mockCommentFactory.Setup(x => x.Create(command.Content, user.Name, postId)).Returns(comment);
        _mockCommentRepository.Setup(x => x.AddAsync(It.IsAny<Comment>(), postId)).Returns(Task.CompletedTask);
        _mockCommentRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        await _commentService.AddCommentAsync(command, userId);

        // Assert
        _mockCommentRepository.Verify(x => x.AddAsync(It.IsAny<Comment>(), postId), Times.Once);
        _mockNotificationService.Verify(x => x.SendAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<TypeNotification>()), Times.Never);
    }

    [Fact]
    public async Task AddCommentAsync_WithNonExistingUser_ShouldThrowException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new AddCommentCommand
        {
            Content = "Test comment",
            PostId = Guid.NewGuid()
        };

        _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _commentService.AddCommentAsync(command, userId));
    }
}
