using ForoUniversitario.DomainLayer.Posts;

namespace ForoUniversitario.ApplicationLayer.Posts;

public class CreatePostCommand
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public TypePost Type { get; set; }
}
