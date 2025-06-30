using ForoUniversitario.DomainLayer.Posts;

namespace ForoUniversitario.DomainLayer.Factories
{
    public interface IPostFactory
    {
        Post CreatePost(string title, string content, Guid authorId, Guid groupId, TypePost type);
    }
}
