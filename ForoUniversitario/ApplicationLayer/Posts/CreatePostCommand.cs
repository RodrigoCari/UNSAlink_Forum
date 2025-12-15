using ForoUniversitario.DomainLayer.Posts;
using System;
using System.Text.Json.Serialization;

namespace ForoUniversitario.ApplicationLayer.Posts
{
    public class CreatePostCommand
    {
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public required Guid AuthorId { get; set; }

        public required Guid GroupId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required TypePost Type { get; set; }
    }
}
