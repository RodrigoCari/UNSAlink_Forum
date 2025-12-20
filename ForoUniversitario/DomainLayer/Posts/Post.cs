using ForoUniversitario.DomainLayer.Users;
using ForoUniversitario.DomainLayer.Groups;

namespace ForoUniversitario.DomainLayer.Posts;

public class Post
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public PostContent Content { get; private set; } = null!; // NAV

    public Guid AuthorId { get; private set; } // FK
    public User Author { get; private set; } = null!; // NAV

    public Guid GroupId { get; private set; } // FK
    public Group Group { get; private set; } = null!; // NAV

    public TypePost Type { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public List<Comment> Comments { get; private set; } = new();

    private Post() { }

    public Post(Guid id, string title, PostContent content, Guid authorId, Guid groupId, TypePost type)
    {
        Id = id;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Content = content ?? throw new ArgumentNullException(nameof(content));
        AuthorId = authorId;
        GroupId = groupId;
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
    public Guid? SharedPostId { get; private set; }
    public Post? SharedPost { get; private set; }
    public static Post CreateSharedPost(string title, Guid authorId, Guid groupId, Post originalPost)
    {
        return new Post(Guid.NewGuid(), title, originalPost.Content, authorId, groupId, TypePost.Shared)
        {
            SharedPostId = originalPost.Id
        };
    }
}
