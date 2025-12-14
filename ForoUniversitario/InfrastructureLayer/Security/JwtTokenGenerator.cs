using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ForoUniversitario.ApplicationLayer.Security;
using ForoUniversitario.DomainLayer.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ForoUniversitario.InfrastructureLayer.Security;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var keyString = jwtSection["Key"];

        if (string.IsNullOrEmpty(keyString))
        {
            throw new Exception("JWT Key is not configured in appsettings.json");
        }

        var key = Encoding.ASCII.GetBytes(keyString);
        var expiresInMinutes = jwtSection["ExpiresInMinutes"];
        
        if (string.IsNullOrEmpty(expiresInMinutes))
        {
            throw new Exception("JWT ExpiresInMinutes is not configured");
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(expiresInMinutes)),
            Issuer = jwtSection["Issuer"],
            Audience = jwtSection["Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
