using ForoUniversitario.DomainLayer.Posts;

namespace ForoUniversitario.DomainLayer.Factories;

public class CommentFactory : ICommentFactory
{
    public Comment Create(string content, string authorName, Guid postId)
    {
        return new Comment(
            id: Guid.NewGuid(),
            content: content,
            author: authorName,
            postId: postId
        );
    }
}