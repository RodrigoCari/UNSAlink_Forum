using ForoUniversitario.ApplicationLayer.Posts;

namespace ForoUniversitario.ApplicationLayer.Groups;

public class GroupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid AdminId { get; set; }
    public PostDto? LatestPost { get; set; }
}
