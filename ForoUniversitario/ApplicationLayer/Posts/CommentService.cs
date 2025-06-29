using ForoUniversitario.DomainLayer.Posts;

namespace ForoUniversitario.ApplicationLayer.Posts;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;

    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task AddCommentAsync(AddCommentCommand command)
    {
        var comment = new Comment(Guid.NewGuid(), command.Content, command.Author);
        await _commentRepository.AddAsync(comment, command.PostId);
        await _commentRepository.SaveChangesAsync();
    }
}
