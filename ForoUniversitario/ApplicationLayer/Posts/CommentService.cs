using ForoUniversitario.ApplicationLayer.Notifications;
using ForoUniversitario.DomainLayer.Notifications;
using ForoUniversitario.DomainLayer.Posts;
using ForoUniversitario.DomainLayer.Users;
using ForoUniversitario.DomainLayer.Factories;
using ForoUniversitario.InfrastructureLayer.Persistence;

namespace ForoUniversitario.ApplicationLayer.Posts;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;
    private readonly INotificationService _notificationService;
    private readonly ICommentFactory _commentFactory;

    public CommentService(
        ICommentRepository commentRepository,
        IUserRepository userRepository,
        IPostRepository postRepository,
        INotificationService notificationService,
        ICommentFactory commentFactory)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _postRepository = postRepository;
        _notificationService = notificationService;
        _commentFactory = commentFactory;
    }

    public async Task AddCommentAsync(AddCommentCommand command, Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new ArgumentException("Usuario no encontrado");

        var post = await _postRepository.GetByIdAsync(command.PostId);
        if (post == null) throw new ArgumentException("Post no encontrado");

        var comment = _commentFactory.Create(command.Content, user.Name, command.PostId);

        await _commentRepository.AddAsync(comment, command.PostId);
        await _commentRepository.SaveChangesAsync();

        if (post.AuthorId != userId)
        {
            var message = $"{user.Name} comentó en tu post: \"{post.Title}\"";
            await _notificationService.SendAsync(post.AuthorId, message, TypeNotification.NewComment);
        }
    }
}
