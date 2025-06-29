using ForoUniversitario.Domain.Posts;
using Microsoft.EntityFrameworkCore;

namespace ForoUniversitario.Infrastructure.Persistence;

public class PostRepository : IPostRepository
{
    private readonly ForumDbContext _context;

    public PostRepository(ForumDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Post post)
    {
        await _context.Posts.AddAsync(post);
    }

    public async Task<Post?> GetByIdAsync(Guid id)
    {
        return await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
