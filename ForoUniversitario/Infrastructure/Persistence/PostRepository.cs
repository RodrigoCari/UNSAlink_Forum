using ForoUniversitario.Domain.Posts;
using Microsoft.EntityFrameworkCore;

namespace ForoUniversitario.Infrastructure.Persistence;

public class PostRepository : IPostRepository
{
    private readonly ForoDbContext _context;

    public PostRepository(ForoDbContext context)
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
