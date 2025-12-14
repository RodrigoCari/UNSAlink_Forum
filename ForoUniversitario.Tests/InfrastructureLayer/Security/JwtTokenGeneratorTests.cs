using ForoUniversitario.DomainLayer.Users;
using ForoUniversitario.InfrastructureLayer.Security;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims; // Need this for Claim types
using System.Linq; // Need this for First()

namespace ForoUniversitario.Tests.InfrastructureLayer.Security;

public class JwtTokenGeneratorTests
{
    private readonly JwtTokenGenerator _generator;
    private readonly Mock<IOptions<JwtSettings>> _mockOptions;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGeneratorTests()
    {
        _jwtSettings = new JwtSettings
        {
            Key = "ThisIsASecretKeyForTestingTheTokenGeneratorOptionPattern",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiresInMinutes = "60"
        };

        _mockOptions = new Mock<IOptions<JwtSettings>>();
        _mockOptions.Setup(o => o.Value).Returns(_jwtSettings);

        // This will fail compilation initially because JwtTokenGenerator expects IConfiguration
        _generator = new JwtTokenGenerator(_mockOptions.Object);
    }

    [Fact]
    public void GenerateToken_WithValidSettings_ShouldReturnToken()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "TestUser", "test@email.com", Role.Student, "hash");

        // Act
        var token = _generator.GenerateToken(user);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);

        var handler = new JwtSecurityTokenHandler();
        var decodedToken = handler.ReadJwtToken(token);

        Assert.Equal(_jwtSettings.Issuer, decodedToken.Issuer);
        Assert.Equal(_jwtSettings.Audience, decodedToken.Audiences.First());
    }
}
