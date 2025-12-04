namespace ForoUniversitario.DomainLayer.Posts;

public class Comment
{
    public Guid Id { get; private set; }
    public string Content { get; private set; } = string.Empty; // INICIALIZAR
    public string Author { get; private set; } = string.Empty; // INICIALIZAR
    public DateTime CreatedAt { get; private set; }

    public Guid PostId { get; private set; }
    public Post Post { get; private set; } = null!;

    private Comment() { }

    public Comment(Guid id, string content, string author, Guid postId)
    {
        Id = id;
        Content = content ?? throw new ArgumentNullException(nameof(content));
        Author = author ?? throw new ArgumentNullException(nameof(author));
        CreatedAt = DateTime.UtcNow;
        PostId = postId;
    }
}
