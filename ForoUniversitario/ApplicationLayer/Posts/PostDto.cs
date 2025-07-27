using ForoUniversitario.DomainLayer.Posts;

namespace ForoUniversitario.ApplicationLayer.Posts;

public class PostDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;

    public Guid GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;

    public TypePost Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CommentDto> Comments { get; set; } = new();
    public PostDto? SharedPost { get; set; }
}
