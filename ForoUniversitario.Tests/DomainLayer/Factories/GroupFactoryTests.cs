namespace ForoUniversitario.Tests.DomainLayer.Factories;

public class GroupFactoryTests
{
    [Fact]
    public void Create_WithValidParameters_ShouldCreateGroup()
    {
        // Arrange
        var factory = new GroupFactory();
        var name = "Test Group";
        var description = "Test Description";
        var admin = new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash");

        // Act
        var group = factory.Create(name, description, admin);

        // Assert
        Assert.NotNull(group);
        Assert.Equal(name, group.Name);
        Assert.Equal(description, group.Description);
        Assert.Equal(admin, group.Admin);
    }
}
