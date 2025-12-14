using ForoUniversitario.DomainLayer.Posts;
using Microsoft.AspNetCore.Identity;

namespace ForoUniversitario.DomainLayer.Users;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public Role Role { get; private set; }

    private readonly List<Post> _posts = new();
    public IReadOnlyCollection<Post> Posts => _posts.AsReadOnly(); // Encapsulation
    public List<string> Interests { get; private set; } = new();

    private User() { }

    public User(Guid id, string name, string email, Role role, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentNullException(nameof(passwordHash));

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

    public void AddPost(Post post)
    {
        _posts.Add(post);
    }
}
