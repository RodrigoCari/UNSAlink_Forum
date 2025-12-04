using ForoUniversitario.DomainLayer.Users;
using ForoUniversitario.InfrastructureLayer.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ForoUniversitario.Tests.InfrastructureLayer.Persistence;

public class UserRepositoryTests : IDisposable
{
    private readonly ForumDbContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ForumDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ForumDbContext(options);
        _repository = new UserRepository(_context);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    [Fact]
    public async Task AddAsync_WithValidUser_ShouldAddToDatabase()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "Test User", "test@email.com", Role.Student, "hash");

        // Act
        await _repository.AddAsync(user);

        // Assert
        var savedUser = await _context.Users.FindAsync(user.Id);
        Assert.NotNull(savedUser);
        Assert.Equal(user.Name, savedUser.Name);
        Assert.Equal(user.Email, savedUser.Email);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingUser_ShouldReturnUser()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "Test User", "test@email.com", Role.Student, "hash");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Name, result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingUser_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ModifyAsync_WithExistingUser_ShouldUpdateUser()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "Old Name", "old@email.com", Role.Student, "hash");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        user.UpdateProfile("New Name", "new@email.com");

        // Act
        await _repository.ModifyAsync(user);

        // Assert
        var updatedUser = await _context.Users.FindAsync(user.Id);
        Assert.NotNull(updatedUser);
        Assert.Equal("New Name", updatedUser.Name);
        Assert.Equal("new@email.com", updatedUser.Email);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingUser_ShouldRemoveUser()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "Test User", "test@email.com", Role.Student, "hash");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(user.Id);

        // Assert
        var deletedUser = await _context.Users.FindAsync(user.Id);
        Assert.Null(deletedUser);
    }

    [Fact]
    public async Task GetByNameAsync_WithExistingUser_ShouldReturnUser()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "SpecificUser", "specific@email.com", Role.Student, "hash");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByNameAsync("SpecificUser");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Name, result.Name);
    }
}
