namespace ForoUniversitario.DomainLayer.Posts;

public class Post
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public PostContent Content { get; private set; }
    public string Author { get; private set; }
    public TypePost Type { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public List<Comment> Comments { get; private set; } = new();

    private Post() { } // Required by EF

    public Post(Guid id, string title, PostContent content, string author, TypePost type)
    {
        Id = id;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Content = content ?? throw new ArgumentNullException(nameof(content));
        Author = author ?? throw new ArgumentNullException(nameof(author));
        Type = type;
        CreatedAt = DateTime.UtcNow;
    }

    public void EditContent(string newText)
    {
        Content = new PostContent(newText);
    }

    public void ChangeType(TypePost newType)
    {
        Type = newType;
    }

    public void AddComment(Comment comment)
    {
        Comments.Add(comment);
    }
}
