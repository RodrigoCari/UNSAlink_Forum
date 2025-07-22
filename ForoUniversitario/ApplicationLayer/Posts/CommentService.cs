using ForoUniversitario.DomainLayer.Posts;
using ForoUniversitario.DomainLayer.Users;

namespace ForoUniversitario.ApplicationLayer.Posts;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;

    public CommentService(ICommentRepository commentRepository, IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
    }

    public async Task AddCommentAsync(AddCommentCommand command, Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new ArgumentException("Usuario no encontrado");

        var comment = new Comment(Guid.NewGuid(), command.Content, user.Name, command.PostId);
        await _commentRepository.AddAsync(comment, command.PostId);
        await _commentRepository.SaveChangesAsync();
    }
}
