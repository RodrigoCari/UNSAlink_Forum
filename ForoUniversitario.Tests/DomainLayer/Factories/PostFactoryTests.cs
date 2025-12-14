namespace ForoUniversitario.Tests.DomainLayer.Factories;

public class PostFactoryTests
{
    [Fact]
    public void CreatePost_WithValidParameters_ShouldCreatePost()
    {
        // Arrange
        var factory = new PostFactory();
        var title = "Test Post";
        var content = "Test Content";
        var authorId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var type = TypePost.Discussion;

        // Act
        var post = factory.CreatePost(title, content, authorId, groupId, type);

        // Assert
        Assert.NotNull(post);
        Assert.Equal(title, post.Title);
        Assert.Equal(content, post.Content.Text);
        Assert.Equal(authorId, post.AuthorId);
        Assert.Equal(groupId, post.GroupId);
        Assert.Equal(type, post.Type);
    }

    [Fact]
    public void CreatePost_WithNullTitle_ShouldThrowArgumentNullException()
    {
        // Arrange
        var factory = new PostFactory();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            factory.CreatePost(null!, "content", Guid.NewGuid(), Guid.NewGuid(), TypePost.Discussion));
    }

    [Fact]
    public void CreatePost_WithNullContent_ShouldThrowArgumentException()
    {
        // Arrange
        var factory = new PostFactory();

        // Act & Assert - PostContent lanza ArgumentException, no ArgumentNullException
        var exception = Assert.Throws<ArgumentException>(() =>
            factory.CreatePost("title", null!, Guid.NewGuid(), Guid.NewGuid(), TypePost.Discussion));

        Assert.Contains("Content cannot be empty", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void CreatePost_WithEmptyOrWhitespaceTitle_ShouldCreatePost(string invalidTitle)
    {
        // Arrange
        var factory = new PostFactory();

        // Act - NO debería lanzar excepción según implementación actual de Post
        var post = factory.CreatePost(invalidTitle, "content", Guid.NewGuid(), Guid.NewGuid(), TypePost.Discussion);

        // Assert
        Assert.NotNull(post);
        Assert.Equal(invalidTitle, post.Title);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void CreatePost_WithEmptyOrWhitespaceContent_ShouldThrowArgumentException(string invalidContent)
    {
        // Arrange
        var factory = new PostFactory();

        // Act & Assert - PostContent SI lanza excepción para contenido vacío
        var exception = Assert.Throws<ArgumentException>(() =>
            factory.CreatePost("title", invalidContent, Guid.NewGuid(), Guid.NewGuid(), TypePost.Discussion));

        Assert.Contains("Content cannot be empty", exception.Message);
    }

    [Fact]
    public void CreatePost_WithContentExceedingMaxLength_ShouldThrowArgumentException()
    {
        // Arrange
        var factory = new PostFactory();
        var longContent = new string('a', 1001); // 1001 caracteres

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            factory.CreatePost("title", longContent, Guid.NewGuid(), Guid.NewGuid(), TypePost.Discussion));

        Assert.Contains("Content cannot exceed 1000 characters", exception.Message);
    }

    [Fact]
    public void CreatePost_GeneratesUniqueId_EachTime()
    {
        // Arrange
        var factory = new PostFactory();

        // Act
        var post1 = factory.CreatePost("Title 1", "Content 1", Guid.NewGuid(), Guid.NewGuid(), TypePost.Discussion);
        var post2 = factory.CreatePost("Title 2", "Content 2", Guid.NewGuid(), Guid.NewGuid(), TypePost.Discussion);

        // Assert
        Assert.NotNull(post1);
        Assert.NotNull(post2);
        Assert.NotEqual(post1.Id, post2.Id);
    }
}
