namespace ForoUniversitario.DomainLayer.Posts;

public interface ICommentRepository
{
    Task AddAsync(Comment comment, Guid postId);
    Task<IEnumerable<Comment>> GetByPostIdAsync(Guid postId);
    Task DeleteAsync(Guid commentId);
    Task SaveChangesAsync();
}
