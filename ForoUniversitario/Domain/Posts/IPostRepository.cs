namespace ForoUniversitario.Domain.Posts;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(Guid id);
    Task AddAsync(Post post);
    Task SaveChangesAsync();
}
