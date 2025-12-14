using ForoUniversitario.DomainLayer.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ForoUniversitario.ApplicationLayer.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task<Guid> RegisterAsync(RegisterUserCommand command)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);
        var user = new User(Guid.NewGuid(), command.Name, command.Email, command.Role, passwordHash);
        await _repository.AddAsync(user);
        return user.Id;
    }

    public async Task UpdateProfileAsync(Guid id, UpdateUserProfileCommand command)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) throw new Exception("User not found");

        user.UpdateProfile(command.Name, command.Email);
        user.UpdateInterests(command.Interests);
        await _repository.ModifyAsync(user);
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        return user == null ? null : new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            Interests = user.Interests
        };
    }

    public async Task<List<string>> GetWorksAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) throw new Exception("User not found");

        return new List<string> { "Work A", "Work B", "Work C" };
    }

    public async Task<string> LoginAsync(LoginUserCommand command)
    {
        var user = await _repository.GetByNameAsync(command.Name);
        if (user == null || !BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

        var jwtSection = _configuration.GetSection("Jwt");
        var keyString = jwtSection["Key"];

        if (string.IsNullOrEmpty(keyString))
        {
            throw new Exception("JWT Key is not configured in appsettings.json");
        }

        var key = Encoding.ASCII.GetBytes(keyString);

        // CORREGIR: Validar que ExpiresInMinutes no sea nulo
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
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(expiresInMinutes)), // Ya no da warning
            Issuer = jwtSection["Issuer"],
            Audience = jwtSection["Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
