using ForoUniversitario.DomainLayer.Posts;
using System;

namespace ForoUniversitario.DomainLayer.Factories
{
    public class PostFactory : IPostFactory
    {
        public Post CreatePost(string title, string contentText, Guid authorId, Guid groupId, TypePost type)
        {
            var content = new PostContent(contentText);
            return new Post(
                Guid.NewGuid(),
                title,
                content,
                authorId,
                groupId,
                type
            );
        }
    }
}
