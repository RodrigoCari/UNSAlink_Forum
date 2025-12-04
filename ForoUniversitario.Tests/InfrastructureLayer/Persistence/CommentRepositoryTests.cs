using ForoUniversitario.DomainLayer.Posts;
using ForoUniversitario.DomainLayer.Users;
using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.InfrastructureLayer.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ForoUniversitario.Tests.InfrastructureLayer.Persistence;

public class CommentRepositoryTests : IDisposable
{
    private readonly ForumDbContext _context;
    private readonly CommentRepository _repository;

    public CommentRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ForumDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ForumDbContext(options);
        _repository = new CommentRepository(_context);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    [Fact]
    public async Task AddAsync_WithValidComment_ShouldAddToDatabase()
    {
        // Arrange
        var author = new User(Guid.NewGuid(), "Author", "author@email.com", Role.Student, "hash");
        var group = new Group(Guid.NewGuid(), "Group", "Description", author);
        var post = new Post(Guid.NewGuid(), "Test Post", new PostContent("Test Content"), author.Id, group.Id, TypePost.Discussion);

        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();

        var comment = new Comment(Guid.NewGuid(), "Great post!", "Commenter", post.Id);

        // Act
        await _repository.AddAsync(comment, post.Id);
        await _repository.SaveChangesAsync();

        // Assert
        var savedComment = await _context.Comments.FindAsync(comment.Id);
        Assert.NotNull(savedComment);
        Assert.Equal(comment.Content, savedComment.Content);
        Assert.Equal(comment.Author, savedComment.Author);
    }

    [Fact]
    public async Task GetByPostIdAsync_WithComments_ShouldReturnComments()
    {
        // Arrange
        var author = new User(Guid.NewGuid(), "Author", "author@email.com", Role.Student, "hash");
        var group = new Group(Guid.NewGuid(), "Group", "Description", author);
        var post = new Post(Guid.NewGuid(), "Test Post", new PostContent("Test Content"), author.Id, group.Id, TypePost.Discussion);

        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();

        var comment1 = new Comment(Guid.NewGuid(), "Comment 1", "User1", post.Id);
        var comment2 = new Comment(Guid.NewGuid(), "Comment 2", "User2", post.Id);

        await _context.Comments.AddRangeAsync(comment1, comment2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByPostIdAsync(post.Id);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, c => c.Content == "Comment 1");
        Assert.Contains(result, c => c.Content == "Comment 2");
    }

    [Fact]
    public async Task DeleteAsync_WithExistingComment_ShouldRemoveComment()
    {
        // Arrange
        var author = new User(Guid.NewGuid(), "Author", "author@email.com", Role.Student, "hash");
        var group = new Group(Guid.NewGuid(), "Group", "Description", author);
        var post = new Post(Guid.NewGuid(), "Test Post", new PostContent("Test Content"), author.Id, group.Id, TypePost.Discussion);
        var comment = new Comment(Guid.NewGuid(), "Test Comment", "Commenter", post.Id);

        await _context.Posts.AddAsync(post);
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(comment.Id);
        await _repository.SaveChangesAsync();

        // Assert
        var deletedComment = await _context.Comments.FindAsync(comment.Id);
        Assert.Null(deletedComment);
    }
}
