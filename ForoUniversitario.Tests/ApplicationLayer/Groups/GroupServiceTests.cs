namespace ForoUniversitario.Tests.ApplicationLayer.Groups;

public class GroupServiceTests
{
    private readonly Mock<IGroupRepository> _mockGroupRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IGroupFactory> _mockGroupFactory;
    private readonly Mock<IGroupDomainService> _mockGroupDomainService;
    private readonly Mock<IPostRepository> _mockPostRepository;
    private readonly GroupService _groupService;

    public GroupServiceTests()
    {
        _mockGroupRepository = new Mock<IGroupRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockGroupFactory = new Mock<IGroupFactory>();
        _mockGroupDomainService = new Mock<IGroupDomainService>();
        _mockPostRepository = new Mock<IPostRepository>();

        _groupService = new GroupService(
            _mockGroupRepository.Object,
            _mockUserRepository.Object,
            _mockGroupFactory.Object,
            _mockGroupDomainService.Object,
            _mockPostRepository.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidCommand_ShouldReturnGroupId()
    {
        // Arrange
        var adminId = Guid.NewGuid();
        var admin = new User(adminId, "Admin", "admin@email.com", Role.Teacher, "hash");
        var command = new CreateGroupCommand
        {
            Name = "Test Group",
            Description = "Test Description",
            AdminId = adminId
        };
        var group = new Group(Guid.NewGuid(), command.Name, command.Description, admin);

        _mockUserRepository.Setup(x => x.GetByIdAsync(adminId)).ReturnsAsync(admin);
        _mockGroupFactory.Setup(x => x.Create(command.Name, command.Description, admin)).Returns(group);
        _mockGroupRepository.Setup(x => x.CreateAsync(group)).Returns(Task.CompletedTask);
        _mockGroupRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _groupService.CreateAsync(command);

        // Assert
        Assert.Equal(group.Id, result);
        _mockUserRepository.Verify(x => x.GetByIdAsync(adminId), Times.Once);
        _mockGroupFactory.Verify(x => x.Create(command.Name, command.Description, admin), Times.Once);
        _mockGroupRepository.Verify(x => x.CreateAsync(group), Times.Once);
        _mockGroupRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithNonExistingAdmin_ShouldThrowException()
    {
        // Arrange
        var command = new CreateGroupCommand
        {
            Name = "Test Group",
            Description = "Test Description",
            AdminId = Guid.NewGuid()
        };

        _mockUserRepository.Setup(x => x.GetByIdAsync(command.AdminId)).ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _groupService.CreateAsync(command));
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingGroup_ShouldReturnGroupDto()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var group = new Group(groupId, "Test Group", "Test Description",
            new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash"));

        _mockGroupRepository.Setup(x => x.FindAsync(groupId)).ReturnsAsync(group);

        // Act
        var result = await _groupService.GetByIdAsync(groupId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(group.Id, result.Id);
        Assert.Equal(group.Name, result.Name);
        Assert.Equal(group.Description, result.Description);
        Assert.Equal(group.AdminId, result.AdminId);
    }

    [Fact]
    public async Task JoinAsync_WithValidUserAndGroup_ShouldAddMember()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var user = new User(userId, "User", "user@email.com", Role.Student, "hash");
        var group = new Group(groupId, "Group", "Description",
            new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash"));

        _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);
        _mockGroupRepository.Setup(x => x.FindAsync(groupId)).ReturnsAsync(group);
        _mockGroupDomainService.Setup(x => x.AddMemberAsync(group, user)).Returns(Task.CompletedTask);
        _mockGroupRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        await _groupService.JoinAsync(groupId, userId);

        // Assert
        _mockGroupDomainService.Verify(x => x.AddMemberAsync(group, user), Times.Once);
        _mockGroupRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_WithName_ShouldReturnMatchingGroups()
    {
        // Arrange
        var searchName = "Study";
        var groups = new List<Group>
        {
            new Group(Guid.NewGuid(), "Study Group 1", "Description 1",
                new User(Guid.NewGuid(), "Admin1", "admin1@email.com", Role.Teacher, "hash")),
            new Group(Guid.NewGuid(), "Study Group 2", "Description 2",
                new User(Guid.NewGuid(), "Admin2", "admin2@email.com", Role.Teacher, "hash"))
        };

        _mockGroupRepository.Setup(x => x.SearchByNameAsync(searchName)).ReturnsAsync(groups);

        // Act
        var result = await _groupService.SearchAsync(searchName);

        // Assert
        Assert.Equal(2, result.Count());
        _mockGroupRepository.Verify(x => x.SearchByNameAsync(searchName), Times.Once);
    }
}
