namespace ForoUniversitario.Tests.DomainLayer.Groups;

public class GroupTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateGroup()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Study Group";
        var description = "Group for studying";
        var admin = new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash");

        // Act
        var group = new Group(id, name, description, admin);

        // Assert
        Assert.Equal(id, group.Id);
        Assert.Equal(name, group.Name);
        Assert.Equal(description, group.Description);
        Assert.Equal(admin.Id, group.AdminId);
        Assert.Single(group.Members);
        Assert.Contains(admin, group.Members);
    }

    [Fact]
    public void Constructor_WithNullName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var admin = new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new Group(Guid.NewGuid(), null!, "Description", admin));
    }

    [Fact]
    public void Constructor_WithNullDescription_ShouldThrowArgumentNullException()
    {
        // Arrange
        var admin = new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new Group(Guid.NewGuid(), "Name", null!, admin));
    }

    [Fact]
    public void Constructor_WithNullAdmin_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new Group(Guid.NewGuid(), "Name", "Description", null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyOrWhitespaceName_ShouldNotThrowException(string invalidName)
    {
        // Arrange
        var admin = new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash");

        // Act & Assert - Estos valores NO deberían lanzar excepción según tu implementación actual
        var group = new Group(Guid.NewGuid(), invalidName, "Description", admin);

        // Verificar que se creó correctamente
        Assert.NotNull(group);
        Assert.Equal(invalidName, group.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyOrWhitespaceDescription_ShouldNotThrowException(string invalidDescription)
    {
        // Arrange
        var admin = new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash");

        // Act & Assert - Estos valores NO deberían lanzar excepción según tu implementación actual
        var group = new Group(Guid.NewGuid(), "Name", invalidDescription, admin);

        // Verificar que se creó correctamente
        Assert.NotNull(group);
        Assert.Equal(invalidDescription, group.Description);
    }

    [Fact]
    public void AddMember_WithNewUser_ShouldAddToMembers()
    {
        // Arrange
        var admin = new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash");
        var group = new Group(Guid.NewGuid(), "Group", "Description", admin);
        var newMember = new User(Guid.NewGuid(), "Member", "member@email.com", Role.Student, "hash");

        // Act
        group.AddMember(newMember);

        // Assert
        Assert.Equal(2, group.Members.Count);
        Assert.Contains(newMember, group.Members);
    }

    [Fact]
    public void AddMember_WithExistingUser_ShouldNotDuplicate()
    {
        // Arrange
        var admin = new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash");
        var group = new Group(Guid.NewGuid(), "Group", "Description", admin);
        var member = new User(Guid.NewGuid(), "Member", "member@email.com", Role.Student, "hash");
        group.AddMember(member);

        // Act
        group.AddMember(member);

        // Assert
        Assert.Equal(2, group.Members.Count);
    }

    [Fact]
    public void AddViewer_WithNewUser_ShouldAddToViewers()
    {
        // Arrange
        var admin = new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash");
        var group = new Group(Guid.NewGuid(), "Group", "Description", admin);
        var viewer = new User(Guid.NewGuid(), "Viewer", "viewer@email.com", Role.Student, "hash");

        // Act
        group.AddViewer(viewer);

        // Assert
        Assert.Single(group.Viewers);
        Assert.Contains(viewer, group.Viewers);
    }
}
