namespace ForoUniversitario.DomainLayer.Groups;

public interface IGroupRepository
{
    Task CreateAsync(Group group);
    Task DeleteAsync(Guid groupId);
    Task<Group?> FindAsync(Guid groupId);
    Task<IEnumerable<Group>> SearchByNameAsync(string name);
    Task JoinAsync(Guid groupId, Guid userId);
    Task LeaveAsync(Guid groupId, Guid userId);
    Task SaveChangesAsync();
    Task<IEnumerable<Group>> GetGroupsByMemberAsync(Guid userId);
    Task<IEnumerable<Group>> GetAllAsync();
}
