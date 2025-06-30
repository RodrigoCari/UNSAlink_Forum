using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.DomainLayer.Users;

namespace ForoUniversitario.ApplicationLayer.Groups;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _repository;
    private readonly IUserRepository _userRepository;

    public GroupService(IGroupRepository repository, IUserRepository userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task<Guid> CreateAsync(CreateGroupCommand command)
    {
        var admin = await _userRepository.GetByIdAsync(command.AdminId);
        if (admin == null)
            throw new InvalidOperationException("Admin user not found.");

        var group = new Group(Guid.NewGuid(), command.Name, command.Description, command.AdminId);
        group.AddMember(admin); // Opcional: el admin es también miembro

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
            Description = group.Description,
            AdminId = group.AdminId
        };
    }

    public async Task JoinAsync(Guid groupId, Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        var group = await _repository.FindAsync(groupId);

        if (user == null || group == null)
            throw new InvalidOperationException("Group or user not found.");

        group.AddMember(user);
        await _repository.SaveChangesAsync();
    }

    public async Task<IEnumerable<GroupDto>> SearchAsync(string name)
    {
        var groups = await _repository.SearchByNameAsync(name);
        return groups.Select(g => new GroupDto
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description,
            AdminId = g.AdminId
        });
    }
}
