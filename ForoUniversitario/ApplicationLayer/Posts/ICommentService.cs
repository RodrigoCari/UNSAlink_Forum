using ForoUniversitario.ApplicationLayer.Posts;

namespace ForoUniversitario.ApplicationLayer.Posts;

public interface ICommentService
{
    Task AddCommentAsync(AddCommentCommand command);
}
