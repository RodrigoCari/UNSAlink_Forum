using ForoUniversitario.DomainLayer.Posts;
using ForoUniversitario.DomainLayer.Users;
using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.InfrastructureLayer.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ForoUniversitario.Tests.InfrastructureLayer.Persistence;

public class PostRepositoryTests : IDisposable
{
    private readonly ForumDbContext _context;
    private readonly PostRepository _repository;

    public PostRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ForumDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ForumDbContext(options);
        _repository = new PostRepository(_context);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    [Fact]
    public async Task AddAsync_WithValidPost_ShouldAddToDatabase()
    {
        // Arrange
        var author = new User(Guid.NewGuid(), "Author", "author@email.com", Role.Student, "hash");
        var group = new Group(Guid.NewGuid(), "Group", "Description", author);

        await _context.Users.AddAsync(author);
        await _context.Set<Group>().AddAsync(group);
        await _context.SaveChangesAsync();

        var post = new Post(Guid.NewGuid(), "Test Post", new PostContent("Test Content"), author.Id, group.Id, TypePost.Discussion);

        // Act
        await _repository.AddAsync(post);
        await _repository.SaveChangesAsync();

        // Assert
        var savedPost = await _context.Posts.FindAsync(post.Id);
        Assert.NotNull(savedPost);
        Assert.Equal(post.Title, savedPost.Title);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingPost_ShouldReturnPost()
    {
        // Arrange
        var author = new User(Guid.NewGuid(), "Author", "author@email.com", Role.Student, "hash");
        var group = new Group(Guid.NewGuid(), "Group", "Description", author);

        await _context.Users.AddAsync(author);
        await _context.Set<Group>().AddAsync(group);
        await _context.SaveChangesAsync();

        var post = new Post(Guid.NewGuid(), "Test Post", new PostContent("Test Content"), author.Id, group.Id, TypePost.Discussion);

        // Usar el contexto directamente para evitar problemas con Include
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(post.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(post.Id, result.Id);
        Assert.Equal(post.Title, result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingPost_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByTypeAsync_WithMatchingType_ShouldReturnPosts()
    {
        // Arrange
        var author = new User(Guid.NewGuid(), "Author", "author@email.com", Role.Student, "hash");
        var group = new Group(Guid.NewGuid(), "Group", "Description", author);

        await _context.Users.AddAsync(author);
        await _context.Set<Group>().AddAsync(group);
        await _context.SaveChangesAsync();

        var post1 = new Post(Guid.NewGuid(), "Post 1", new PostContent("Content 1"), author.Id, group.Id, TypePost.Discussion);
        var post2 = new Post(Guid.NewGuid(), "Post 2", new PostContent("Content 2"), author.Id, group.Id, TypePost.EducationalContributions);

        await _context.Posts.AddRangeAsync(post1, post2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByTypeAsync(TypePost.Discussion);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, p => p.Title == "Post 1");
    }

    [Fact]
    public async Task GetPostsByUserAsync_WithUserPosts_ShouldReturnPosts()
    {
        // Arrange
        var author = new User(Guid.NewGuid(), "Author", "author@email.com", Role.Student, "hash");
        var group = new Group(Guid.NewGuid(), "Group", "Description", author);

        await _context.Users.AddAsync(author);
        await _context.Set<Group>().AddAsync(group);
        await _context.SaveChangesAsync();

        var post = new Post(Guid.NewGuid(), "Test Post", new PostContent("Test Content"), author.Id, group.Id, TypePost.Discussion);

        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetPostsByUserAsync(author.Id);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, p => p.Title == "Test Post");
    }

    [Fact]
    public async Task GetByGroupAsync_WithGroupPosts_ShouldReturnPosts()
    {
        // Arrange
        var author = new User(Guid.NewGuid(), "Author", "author@email.com", Role.Student, "hash");
        var group = new Group(Guid.NewGuid(), "Group", "Description", author);

        await _context.Users.AddAsync(author);
        await _context.Set<Group>().AddAsync(group);
        await _context.SaveChangesAsync();

        var post = new Post(Guid.NewGuid(), "Test Post", new PostContent("Test Content"), author.Id, group.Id, TypePost.Discussion);

        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByGroupAsync(group.Id);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, p => p.Title == "Test Post");
    }
}
