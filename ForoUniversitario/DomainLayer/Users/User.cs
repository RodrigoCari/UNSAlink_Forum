using ForoUniversitario.DomainLayer.Posts;
using Microsoft.AspNetCore.Identity;

namespace ForoUniversitario.DomainLayer.Users;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; }
    public Role Role { get; private set; }

    public List<Post> Posts { get; private set; } = new();
    public List<string> Interests { get; private set; } = new();

    private User() { }

    public User(Guid id, string name, string email, Role role, string passwordHash)
    {
        Id = id;
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }

    public void UpdateProfile(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public void UpdateInterests(List<string> interests)
    {
        Interests = interests;
    }
}
