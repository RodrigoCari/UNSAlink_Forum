using ForoUniversitario.DomainLayer.Users;
using Microsoft.EntityFrameworkCore;

namespace ForoUniversitario.InfrastructureLayer.Persistence;

public class UserRepository : IUserRepository
{
    private readonly ForumDbContext _context;

    public UserRepository(ForumDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task ModifyAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<User?> GetByNameAsync(string name)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
    }
}
