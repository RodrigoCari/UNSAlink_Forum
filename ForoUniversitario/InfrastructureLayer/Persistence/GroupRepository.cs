using ForoUniversitario.DomainLayer.Groups;
using Microsoft.EntityFrameworkCore;

namespace ForoUniversitario.InfrastructureLayer.Persistence;

public class GroupRepository : IGroupRepository
{
    private readonly ForumDbContext _context;

    public GroupRepository(ForumDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Group group)
    {
        await _context.Set<Group>().AddAsync(group);
    }

    public async Task DeleteAsync(Guid groupId)
    {
        var group = await FindAsync(groupId);
        if (group != null)
            _context.Set<Group>().Remove(group);
    }

    public async Task<Group?> FindAsync(Guid groupId)
    {
        return await _context.Set<Group>().FindAsync(groupId);
    }

    public async Task<IEnumerable<Group>> SearchByNameAsync(string name)
    {
        return await _context.Set<Group>()
            .Where(g => g.Name.Contains(name))
            .ToListAsync();
    }

    public async Task JoinAsync(Guid groupId, Guid userId)
    {
        // Simulated join (needs a join table in real DB model)
        // Here, assume another structure exists or is mocked.
    }

    public async Task LeaveAsync(Guid groupId, Guid userId)
    {
        // Simulated leave
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
