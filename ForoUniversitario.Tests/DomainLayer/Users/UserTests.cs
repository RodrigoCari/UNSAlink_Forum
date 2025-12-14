namespace ForoUniversitario.Tests.DomainLayer.Users;

public class UserTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateUser()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "John Doe";
        var email = "john@example.com";
        var role = Role.Student;
        var passwordHash = "hashed_password";

        // Act
        var user = new User(id, name, email, role, passwordHash);

        // Assert
        Assert.Equal(id, user.Id);
        Assert.Equal(name, user.Name);
        Assert.Equal(email, user.Email);
        Assert.Equal(role, user.Role);
        Assert.Equal(passwordHash, user.PasswordHash);
        Assert.Empty(user.Posts);
        Assert.Empty(user.Interests);
    }

    [Fact]
    public void UpdateProfile_WithValidData_ShouldUpdateProperties()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "Old Name", "old@email.com", Role.Student, "hash");
        var newName = "New Name";
        var newEmail = "new@email.com";

        // Act
        user.UpdateProfile(newName, newEmail);

        // Assert
        Assert.Equal(newName, user.Name);
        Assert.Equal(newEmail, user.Email);
    }

    [Fact]
    public void UpdateInterests_WithValidList_ShouldUpdateInterests()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "Test User", "test@email.com", Role.Student, "hash");
        var interests = new List<string> { "Programming", "Math", "Science" };

        // Act
        user.UpdateInterests(interests);

        // Assert
        Assert.Equal(3, user.Interests.Count);
        Assert.Contains("Programming", user.Interests);
        Assert.Contains("Math", user.Interests);
        Assert.Contains("Science", user.Interests);
    }

    [Fact]
    public void UpdateInterests_WithEmptyList_ShouldClearInterests()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "Test User", "test@email.com", Role.Student, "hash");
        user.UpdateInterests(new List<string> { "Initial" });

        // Act
        user.UpdateInterests(new List<string>());

        // Assert
        Assert.Empty(user.Interests);
    }
}
