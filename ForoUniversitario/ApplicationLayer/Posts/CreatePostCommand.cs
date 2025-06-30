using ForoUniversitario.DomainLayer.Posts;

namespace ForoUniversitario.ApplicationLayer.Posts;

public class CreatePostCommand
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    public Guid GroupId { get; set; }
    public TypePost Type { get; set; }
}
