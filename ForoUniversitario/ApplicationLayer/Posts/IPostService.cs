namespace ForoUniversitario.ApplicationLayer.Posts;

public interface IPostService
{
    Task<Guid> CreateAsync(CreatePostCommand command);
    Task<PostDto?> GetByIdAsync(Guid id);
}
