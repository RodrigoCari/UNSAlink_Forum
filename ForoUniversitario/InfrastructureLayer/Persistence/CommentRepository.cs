using ForoUniversitario.DomainLayer.Posts;
using Microsoft.EntityFrameworkCore;

namespace ForoUniversitario.InfrastructureLayer.Persistence;

public class CommentRepository : ICommentRepository
{
    private readonly ForumDbContext _context;

    public CommentRepository(ForumDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Comment comment, Guid postId)
    {
        var postExists = await _context.Posts.AnyAsync(p => p.Id == postId);
        if (!postExists) throw new InvalidOperationException("Post not found.");

        _context.Comments.Add(comment);
    }

    public async Task<IEnumerable<Comment>> GetByPostIdAsync(Guid postId)
    {
        var post = await _context.Posts.Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == postId);
        return post?.Comments ?? Enumerable.Empty<Comment>();
    }

    public async Task DeleteAsync(Guid commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment != null) _context.Comments.Remove(comment);
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
