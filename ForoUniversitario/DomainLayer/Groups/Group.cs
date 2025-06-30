using ForoUniversitario.DomainLayer.Users;
using ForoUniversitario.DomainLayer.Posts;

namespace ForoUniversitario.DomainLayer.Groups;

public class Group
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public Guid AdminId { get; private set; } // FK
    public User Admin { get; private set; } = null!;

    public List<User> Members { get; private set; } = new();
    public List<User> Viewers { get; private set; } = new();

    public List<Post> Posts { get; private set; } = new();

    private Group() { }

    public Group(Guid id, string name, string description, Guid adminId)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        AdminId = adminId;
    }

    public void AddMember(User user)
    {
        if (!Members.Contains(user))
            Members.Add(user);
    }

    public void AddViewer(User user)
    {
        if (!Viewers.Contains(user))
            Viewers.Add(user);
    }
}
