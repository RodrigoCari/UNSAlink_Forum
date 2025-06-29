namespace ForoUniversitario.ApplicationLayer.Posts;

public interface IPostService
{
    Task<Guid> CreateAsync(CreatePostCommand command);
    Task<PostDto?> GetByIdAsync(Guid id);
    Task ShareToGroupAsync(Guid postId, Guid groupId);
    Task RequestIdeasAsync(Guid postId);
    Task<IEnumerable<PostDto>> GetByTypeAsync(int type);
}
