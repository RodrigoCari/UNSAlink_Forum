using System;
using System.ComponentModel.DataAnnotations;

namespace ForoUniversitario.ApplicationLayer.Posts
{
    public class SharePostCommand
    {
        [Required]
        public Guid OriginalPostId { get; set; }

        [Required]
        public Guid AuthorId { get; set; }

        [Required]
        public Guid GroupId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int Type { get; set; }
    }
}
