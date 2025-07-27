public class SharePostCommand
{
    public Guid OriginalPostId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid GroupId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Type { get; set; }
}
