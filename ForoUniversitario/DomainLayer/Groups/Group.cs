using ForoUniversitario.DomainLayer.Posts;

namespace ForoUniversitario.DomainLayer.Groups;

public class Group
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    public List<Post> Posts { get; private set; } = new(); // Nueva relación

    private Group() { }

    public Group(Guid id, string name, string description)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }
}
