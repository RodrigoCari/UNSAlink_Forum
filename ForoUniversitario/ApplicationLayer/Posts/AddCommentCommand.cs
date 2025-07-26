namespace ForoUniversitario.ApplicationLayer.Posts;

public class AddCommentCommand
{
    public Guid PostId { get; set; }
    public string Content { get; set; } = string.Empty;
}
