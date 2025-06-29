using ForoUniversitario.DomainLayer.Users;
using System.Data;

namespace ForoUniversitario.ApplicationLayer.Users;

public class RegisterUserCommand
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
}
