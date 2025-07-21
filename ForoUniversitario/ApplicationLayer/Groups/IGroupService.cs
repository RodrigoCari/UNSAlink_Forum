namespace ForoUniversitario.ApplicationLayer.Groups;

public interface IGroupService
{
    Task<Guid> CreateAsync(CreateGroupCommand command);
    Task<GroupDto?> GetByIdAsync(Guid id);
    Task JoinAsync(Guid groupId, Guid userId);
    Task<IEnumerable<GroupDto>> SearchAsync(string name);
    Task<IEnumerable<GroupDto>> GetGroupsByUserAsync(Guid userId);
}
