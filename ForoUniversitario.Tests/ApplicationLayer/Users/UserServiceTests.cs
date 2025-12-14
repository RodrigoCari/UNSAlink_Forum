using Microsoft.Extensions.Configuration;
using Moq;
using ForoUniversitario.ApplicationLayer.Users;
using ForoUniversitario.DomainLayer.Users;

namespace ForoUniversitario.Tests.ApplicationLayer.Users;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IConfigurationSection> _mockJwtSection;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockJwtSection = new Mock<IConfigurationSection>();

        _mockConfiguration.Setup(x => x.GetSection("Jwt")).Returns(_mockJwtSection.Object);
        _mockJwtSection.Setup(x => x["Key"]).Returns("super_secret_key_that_is_long_enough_for_hmac_sha256");
        _mockJwtSection.Setup(x => x["ExpiresInMinutes"]).Returns("60");
        _mockJwtSection.Setup(x => x["Issuer"]).Returns("TestIssuer");
        _mockJwtSection.Setup(x => x["Audience"]).Returns("TestAudience");

        _userService = new UserService(_mockRepository.Object, _mockConfiguration.Object);
    }

    [Fact]
    public async Task RegisterAsync_WithValidCommand_ShouldReturnUserId()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Name = "Test User",
            Email = "test@email.com",
            Password = "password123",
            Role = Role.Student
        };

        User savedUser = null;
        _mockRepository.Setup(x => x.AddAsync(It.IsAny<User>()))
            .Callback<User>(user => savedUser = user)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _userService.RegisterAsync(command);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        _mockRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        Assert.NotNull(savedUser);
        Assert.Equal(command.Name, savedUser.Name);
        Assert.Equal(command.Email, savedUser.Email);
        Assert.Equal(command.Role, savedUser.Role);
        Assert.True(BCrypt.Net.BCrypt.Verify(command.Password, savedUser.PasswordHash));
    }

    [Fact]
    public async Task UpdateProfileAsync_WithExistingUser_ShouldUpdateUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingUser = new User(userId, "Old Name", "old@email.com", Role.Student, "hash");

        // Inicializar intereses usando el método UpdateInterests
        existingUser.UpdateInterests(new List<string> { "Initial" });

        var command = new UpdateUserProfileCommand
        {
            Name = "New Name",
            Email = "new@email.com",
            Interests = new List<string> { "Programming", "Math" }
        };

        _mockRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(existingUser);
        _mockRepository.Setup(x => x.ModifyAsync(existingUser)).Returns(Task.CompletedTask);

        // Act
        await _userService.UpdateProfileAsync(userId, command);

        // Assert
        _mockRepository.Verify(x => x.ModifyAsync(existingUser), Times.Once);
        Assert.Equal("New Name", existingUser.Name);
        Assert.Equal("new@email.com", existingUser.Email);
        Assert.Equal(2, existingUser.Interests.Count);
        Assert.Contains("Programming", existingUser.Interests);
        Assert.Contains("Math", existingUser.Interests);
    }

    [Fact]
    public async Task UpdateProfileAsync_WithNonExistingUser_ShouldThrowException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateUserProfileCommand
        {
            Name = "New Name",
            Email = "new@email.com",
            Interests = new List<string>()
        };

        // CORRECCIÓN FINAL: Usar default
        _mockRepository.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(default(User));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _userService.UpdateProfileAsync(userId, command));
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingUser_ShouldReturnUserDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User(userId, "Test User", "test@email.com", Role.Student, "hash");

        // Usar el método UpdateInterests en lugar de asignación directa
        user.UpdateInterests(new List<string> { "Programming" });

        _mockRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.Role.ToString(), result.Role);
        Assert.Single(result.Interests);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var password = "password123";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User(Guid.NewGuid(), "TestUser", "test@email.com", Role.Student, hashedPassword);

        var command = new LoginUserCommand
        {
            Name = "TestUser",
            Password = password
        };

        _mockRepository.Setup(x => x.GetByNameAsync("TestUser")).ReturnsAsync(user);

        // Act
        var result = await _userService.LoginAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        _mockRepository.Verify(x => x.GetByNameAsync("TestUser"), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidCredentials_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var command = new LoginUserCommand
        {
            Name = "NonExistentUser",
            Password = "wrongpassword"
        };

        // CORRECCIÓN: Cambiar Task.FromResult por ReturnsAsync
        _mockRepository.Setup(x => x.GetByNameAsync("NonExistentUser"))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _userService.LoginAsync(command));
    }
}
