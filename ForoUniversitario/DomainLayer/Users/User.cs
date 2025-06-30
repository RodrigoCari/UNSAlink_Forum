using ForoUniversitario.DomainLayer.Posts;

namespace ForoUniversitario.DomainLayer.Users;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public Role Role { get; private set; }

    public List<Post> Posts { get; private set; } = new();

    private User() { }

    public User(Guid id, string name, string email, Role role)
    {
        Id = id;
        Name = name;
        Email = email;
        Role = role;
    }

    public void UpdateProfile(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
