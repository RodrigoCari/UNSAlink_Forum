using ForoUniversitario.DomainLayer.Users;
using System.ComponentModel.DataAnnotations;

namespace ForoUniversitario.ApplicationLayer.Users;

public class RegisterUserCommand
{
    [Required, MinLength(6)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public Role Role { get; set; }
}
