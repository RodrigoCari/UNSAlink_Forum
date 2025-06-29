namespace ForoUniversitario.DomainLayer.Posts;

public class Comment
{
    public Guid Id { get; private set; }
    public string Content { get; private set; }
    public string Author { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Comment() { }

    public Comment(Guid id, string content, string author)
    {
        Id = id;
        Content = content ?? throw new ArgumentNullException(nameof(content));
        Author = author ?? throw new ArgumentNullException(nameof(author));
        CreatedAt = DateTime.UtcNow;
    }
}
