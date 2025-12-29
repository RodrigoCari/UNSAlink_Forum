using ForoUniversitario.DomainLayer.Posts;

namespace ForoUniversitario.DomainLayer.Factories;

public interface ICommentFactory
{
    Comment Create(string content, string authorName, Guid postId);
}