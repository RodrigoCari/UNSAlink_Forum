using ForoUniversitario.ApplicationLayer.Users;
using ForoUniversitario.ApplicationLayer.Security;
using ForoUniversitario.DomainLayer.Users;
using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace ForoUniversitario.Tests.ApplicationLayer.Users;

public class UserServiceTests
{
    private readonly UserService _userService;
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IJwtTokenGenerator> _mockTokenGenerator;

    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockTokenGenerator = new Mock<IJwtTokenGenerator>();

        // Simular configuración de JWT
        var jwtSectionMock = new Mock<IConfigurationSection>();
        jwtSectionMock.Setup(s => s["Key"]).Returns("ThisIsAOVeryStrongSecretKeyForTheTestEnvironment123!");
        jwtSectionMock.Setup(s => s["Issuer"]).Returns("TestIssuer");
        jwtSectionMock.Setup(s => s["Audience"]).Returns("TestAudience");
        jwtSectionMock.Setup(s => s["ExpiresInMinutes"]).Returns("60");
        _mockConfiguration.Setup(c => c.GetSection("Jwt")).Returns(jwtSectionMock.Object);

        // This will cause compilation error (RED PHASE)
        _userService = new UserService(_mockRepository.Object, _mockConfiguration.Object, _mockTokenGenerator.Object);
    }

    [Fact]
    public async Task RegisterAsync_WithValidCommand_ShouldCreateUser()
    {
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

        var result = await _userService.RegisterAsync(command);

        Assert.NotEqual(Guid.Empty, result);
        _mockRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task UpdateProfileAsync_WithExistingUser_ShouldUpdateUser()
    {
        var userId = Guid.NewGuid();
        var existingUser = new User(userId, "Old Name", "old@email.com", Role.Student, "hash");
        existingUser.UpdateInterests(new List<string> { "Initial" });

        var command = new UpdateUserProfileCommand { Name = "New Name", Email = "new@email.com", Interests = new List<string> { "Programming" } };

        _mockRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(existingUser);
        _mockRepository.Setup(x => x.ModifyAsync(existingUser)).Returns(Task.CompletedTask);

        await _userService.UpdateProfileAsync(userId, command);

        _mockRepository.Verify(x => x.ModifyAsync(existingUser), Times.Once);
        Assert.Equal("New Name", existingUser.Name);
    }

    [Fact]
    public async Task UpdateProfileAsync_WithNonExistingUser_ShouldThrowException()
    {
        var userId = Guid.NewGuid();
        var command = new UpdateUserProfileCommand { Name = "New Name", Email = "new@email.com", Interests = new List<string>() };
        _mockRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(default(User));
        await Assert.ThrowsAsync<Exception>(() => _userService.UpdateProfileAsync(userId, command));
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingUser_ShouldReturnUserDto()
    {
        var userId = Guid.NewGuid();
        var user = new User(userId, "Test User", "test@email.com", Role.Student, "hash");
        user.UpdateInterests(new List<string> { "Programming" });
        _mockRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);

        var result = await _userService.GetByIdAsync(userId);
        Assert.NotNull(result);
        Assert.Equal(user.Name, result.Name);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_DelegatesToTokenGenerator()
    {
        // Arrange
        var password = "password123";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User(Guid.NewGuid(), "TestUser", "test@email.com", Role.Student, hashedPassword);
        var command = new LoginUserCommand { Name = "TestUser", Password = password };

        _mockRepository.Setup(x => x.GetByNameAsync("TestUser")).ReturnsAsync(user);
        _mockTokenGenerator.Setup(x => x.GenerateToken(user)).Returns("mocked-jwt-token");

        // Act
        var result = await _userService.LoginAsync(command);

        // Assert
        Assert.Equal("mocked-jwt-token", result);
        _mockTokenGenerator.Verify(x => x.GenerateToken(user), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_InvalidCredentials_ThrowUnauthorized()
    {
        var command = new LoginUserCommand { Name = "User", Password = "Wrong" };
        _mockRepository.Setup(x => x.GetByNameAsync("User")).ReturnsAsync((User?)null);
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _userService.LoginAsync(command));
    }
}
