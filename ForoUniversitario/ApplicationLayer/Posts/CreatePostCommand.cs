using ForoUniversitario.DomainLayer.Posts;
using System.Text.Json.Serialization;

namespace ForoUniversitario.ApplicationLayer.Posts;

public class CreatePostCommand
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    public Guid GroupId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TypePost Type { get; set; }
}