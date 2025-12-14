using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.DomainLayer.Users;
using ForoUniversitario.InfrastructureLayer.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ForoUniversitario.Tests.InfrastructureLayer.Persistence;

public class GroupRepositoryTests : IDisposable
{
    private readonly ForumDbContext _context;
    private readonly GroupRepository _repository;

    public GroupRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ForumDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ForumDbContext(options);
        _repository = new GroupRepository(_context);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    [Fact]
    public async Task CreateAsync_WithValidGroup_ShouldAddToDatabase()
    {
        // Arrange
        var admin = new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash");
        var group = new Group(Guid.NewGuid(), "Test Group", "Test Description", admin);

        // Act
        await _repository.CreateAsync(group);
        await _repository.SaveChangesAsync();

        // Assert
        var savedGroup = await _context.Set<Group>().FindAsync(group.Id);
        Assert.NotNull(savedGroup);
        Assert.Equal(group.Name, savedGroup.Name);
        Assert.Equal(group.Description, savedGroup.Description);
    }

    [Fact]
    public async Task FindAsync_WithExistingGroup_ShouldReturnGroup()
    {
        // Arrange
        var admin = new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash");
        var group = new Group(Guid.NewGuid(), "Test Group", "Test Description", admin);
        await _context.Set<Group>().AddAsync(group);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.FindAsync(group.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(group.Id, result.Id);
        Assert.Equal(group.Name, result.Name);
    }

    [Fact]
    public async Task FindAsync_WithNonExistingGroup_ShouldReturnNull()
    {
        // Act
        var result = await _repository.FindAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SearchByNameAsync_WithMatchingName_ShouldReturnGroups()
    {
        // Arrange
        var admin = new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash");
        var group1 = new Group(Guid.NewGuid(), "Study Group A", "Description A", admin);
        var group2 = new Group(Guid.NewGuid(), "Study Group B", "Description B", admin);

        await _context.Set<Group>().AddRangeAsync(group1, group2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchByNameAsync("Study");

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, g => g.Name == "Study Group A");
        Assert.Contains(result, g => g.Name == "Study Group B");
    }

    [Fact]
    public async Task GetGroupsByMemberAsync_WithMember_ShouldReturnGroups()
    {
        // Arrange
        var admin = new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash");
        var member = new User(Guid.NewGuid(), "Member", "member@email.com", Role.Student, "hash");

        var group = new Group(Guid.NewGuid(), "Test Group", "Description", admin);
        group.AddMember(member);

        await _context.Set<Group>().AddAsync(group);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetGroupsByMemberAsync(member.Id);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, g => g.Name == "Test Group");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllGroups()
    {
        // Arrange
        var admin = new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash");
        var group1 = new Group(Guid.NewGuid(), "Group 1", "Description 1", admin);
        var group2 = new Group(Guid.NewGuid(), "Group 2", "Description 2", admin);

        await _context.Set<Group>().AddRangeAsync(group1, group2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }
}
