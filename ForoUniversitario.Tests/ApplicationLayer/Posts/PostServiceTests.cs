namespace ForoUniversitario.Tests.ApplicationLayer.Posts;

public class PostServiceTests
{
    private readonly Mock<IPostRepository> _mockPostRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IGroupRepository> _mockGroupRepository;
    private readonly Mock<IPostFactory> _mockPostFactory;
    private readonly Mock<IPostDomainService> _mockPostDomainService;
    private readonly PostService _postService;

    public PostServiceTests()
    {
        _mockPostRepository = new Mock<IPostRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockGroupRepository = new Mock<IGroupRepository>();
        _mockPostFactory = new Mock<IPostFactory>();
        _mockPostDomainService = new Mock<IPostDomainService>();

        _postService = new PostService(
            _mockPostRepository.Object,
            _mockUserRepository.Object,
            _mockGroupRepository.Object,
            _mockPostFactory.Object,
            _mockPostDomainService.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidCommand_ShouldReturnPostId()
    {
        // Arrange
        var command = new CreatePostCommand
        {
            Title = "Test Post",
            Content = "Test Content",
            AuthorId = Guid.NewGuid(),
            GroupId = Guid.NewGuid(),
            Type = TypePost.Discussion
        };
        var post = new Post(Guid.NewGuid(), command.Title, new PostContent(command.Content),
            command.AuthorId, command.GroupId, command.Type);

        _mockPostFactory.Setup(x => x.CreatePost(
            command.Title, command.Content, command.AuthorId, command.GroupId, command.Type))
            .Returns(post);
        _mockPostRepository.Setup(x => x.AddAsync(post)).Returns(Task.CompletedTask);
        _mockPostRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _postService.CreateAsync(command);

        // Assert
        Assert.Equal(post.Id, result);
        _mockPostFactory.Verify(x => x.CreatePost(
            command.Title, command.Content, command.AuthorId, command.GroupId, command.Type), Times.Once);
        _mockPostRepository.Verify(x => x.AddAsync(post), Times.Once);
        _mockPostRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingPost_ShouldReturnPostDto()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var post = new Post(postId, "Test Post", new PostContent("Content"), authorId, groupId, TypePost.Discussion);
        var author = new User(authorId, "Author", "author@email.com", Role.Student, "hash");
        var group = new Group(groupId, "Group", "Description", author);

        _mockPostRepository.Setup(x => x.GetByIdAsync(postId)).ReturnsAsync(post);
        _mockUserRepository.Setup(x => x.GetByIdAsync(authorId)).ReturnsAsync(author);
        _mockGroupRepository.Setup(x => x.FindAsync(groupId)).ReturnsAsync(group);

        // Act
        var result = await _postService.GetByIdAsync(postId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(post.Id, result.Id);
        Assert.Equal(post.Title, result.Title);
        Assert.Equal(post.Content.Text, result.Content);
        Assert.Equal(author.Name, result.AuthorName);
        Assert.Equal(group.Name, result.GroupName);
    }

    [Fact]
    public async Task GetByTypeAsync_WithValidType_ShouldReturnPosts()
    {
        // Arrange
        var type = TypePost.EducationalContributions;
        var posts = new List<Post>
        {
            new Post(Guid.NewGuid(), "Idea 1", new PostContent("Content 1"), Guid.NewGuid(), Guid.NewGuid(), type),
            new Post(Guid.NewGuid(), "Idea 2", new PostContent("Content 2"), Guid.NewGuid(), Guid.NewGuid(), type)
        };

        _mockPostRepository.Setup(x => x.GetByTypeAsync(type)).ReturnsAsync(posts);
        _mockUserRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new User(Guid.NewGuid(), "Author", "email", Role.Student, "hash"));
        _mockGroupRepository.Setup(x => x.FindAsync(It.IsAny<Guid>())).ReturnsAsync(new Group(Guid.NewGuid(), "Group", "Desc", new User(Guid.NewGuid(), "Admin", "admin", Role.Teacher, "hash")));

        // Act
        var result = await _postService.GetByTypeAsync((int)type);

        // Assert
        Assert.Equal(2, result.Count());
        _mockPostRepository.Verify(x => x.GetByTypeAsync(type), Times.Once);
    }

    [Fact]
    public async Task ShareAsync_WithValidCommand_ShouldReturnSharedPostId()
    {
        // Arrange
        var originalPostId = Guid.NewGuid();
        var originalPost = new Post(originalPostId, "Original", new PostContent("Original Content"),
            Guid.NewGuid(), Guid.NewGuid(), TypePost.Discussion);

        var command = new SharePostCommand
        {
            Title = "Shared Post",
            AuthorId = Guid.NewGuid(),
            GroupId = Guid.NewGuid(),
            OriginalPostId = originalPostId
        };

        _mockPostRepository.Setup(x => x.GetByIdAsync(originalPostId)).ReturnsAsync(originalPost);
        _mockPostRepository.Setup(x => x.AddAsync(It.IsAny<Post>())).Returns(Task.CompletedTask);
        _mockPostRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _postService.ShareAsync(command);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        _mockPostRepository.Verify(x => x.GetByIdAsync(originalPostId), Times.Once);
        _mockPostRepository.Verify(x => x.AddAsync(It.Is<Post>(p =>
            p.Type == TypePost.Shared &&
            p.SharedPostId == originalPostId)), Times.Once);
        _mockPostRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}
