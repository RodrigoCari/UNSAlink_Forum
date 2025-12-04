namespace ForoUniversitario.Tests.DomainLayer.DomainServices;

public class GroupDomainServiceTests
{
    [Fact]
    public async Task AddMemberAsync_WithValidUser_ShouldAddMember()
    {
        // Arrange
        var service = new GroupDomainService();
        var admin = new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash");
        var group = new Group(Guid.NewGuid(), "Test Group", "Description", admin);
        var newUser = new User(Guid.NewGuid(), "New User", "newuser@email.com", Role.Student, "hash");

        // Act
        await service.AddMemberAsync(group, newUser);

        // Assert
        Assert.Contains(newUser, group.Members);
    }

    [Fact]
    public void CanUserViewGroup_WhenUserIsMember_ShouldReturnTrue()
    {
        // Arrange
        var service = new GroupDomainService();
        var admin = new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash");
        var group = new Group(Guid.NewGuid(), "Test Group", "Description", admin);
        var user = new User(Guid.NewGuid(), "Member", "member@email.com", Role.Student, "hash");
        group.AddMember(user);

        // Act
        var result = service.CanUserViewGroup(group, user);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanUserViewGroup_WhenUserIsNotMember_ShouldReturnFalse()
    {
        // Arrange
        var service = new GroupDomainService();
        var admin = new User(Guid.NewGuid(), "Admin", "admin@email.com", Role.Teacher, "hash");
        var group = new Group(Guid.NewGuid(), "Test Group", "Description", admin);
        var user = new User(Guid.NewGuid(), "NonMember", "nonmember@email.com", Role.Student, "hash");

        // Act
        var result = service.CanUserViewGroup(group, user);

        // Assert
        Assert.False(result);
    }
}
