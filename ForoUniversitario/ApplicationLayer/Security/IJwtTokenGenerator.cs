using ForoUniversitario.DomainLayer.Users;

namespace ForoUniversitario.ApplicationLayer.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
