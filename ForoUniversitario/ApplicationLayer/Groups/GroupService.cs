using ForoUniversitario.DomainLayer.Groups;

namespace ForoUniversitario.ApplicationLayer.Groups;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _repository;

    public GroupService(IGroupRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateAsync(CreateGroupCommand command)
    {
        var group = new Group(Guid.NewGuid(), command.Name, command.Description);
        await _repository.CreateAsync(group);
        await _repository.SaveChangesAsync();
        return group.Id;
    }

    public async Task<GroupDto?> GetByIdAsync(Guid id)
    {
        var group = await _repository.FindAsync(id);
        return group == null ? null : new GroupDto
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description
        };
    }

    public async Task JoinAsync(Guid groupId, Guid userId)
    {
        await _repository.JoinAsync(groupId, userId);
        await _repository.SaveChangesAsync();
    }

    public async Task<IEnumerable<GroupDto>> SearchAsync(string name)
    {
        var groups = await _repository.SearchByNameAsync(name);
        return groups.Select(g => new GroupDto
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description
        });
    }
}
