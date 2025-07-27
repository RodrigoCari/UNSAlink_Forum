using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.DomainLayer.Users;
using ForoUniversitario.DomainLayer.Posts;
using ForoUniversitario.DomainLayer.Factories;
using ForoUniversitario.DomainLayer.DomainServices;
using ForoUniversitario.ApplicationLayer.Posts;

namespace ForoUniversitario.ApplicationLayer.Groups;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly IGroupFactory _groupFactory;
    private readonly IGroupDomainService _groupDomainService;
    private readonly IPostRepository _postRepository;

    public GroupService(
        IGroupRepository repository,
        IUserRepository userRepository,
        IGroupFactory groupFactory,
        IGroupDomainService groupDomainService,
        IPostRepository postRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
        _groupFactory = groupFactory;
        _groupDomainService = groupDomainService;
        _postRepository = postRepository;
    }

    public async Task<Guid> CreateAsync(CreateGroupCommand command)
    {
        var admin = await _userRepository.GetByIdAsync(command.AdminId);
        if (admin == null)
            throw new InvalidOperationException("Admin user not found.");

        var group = _groupFactory.Create(command.Name, command.Description, admin);

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

        await _groupDomainService.AddMemberAsync(group, user);

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

    public async Task<IEnumerable<GroupDto>> GetGroupsByUserAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new InvalidOperationException("User not found.");

        var groups = await _repository.GetGroupsByMemberAsync(userId);
        return groups.Select(g => new GroupDto
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description,
            AdminId = g.AdminId
        });
    }
    
    public async Task<IEnumerable<GroupDto>> GetAllWithLatestPostAsync()
    {
        var groups = await _repository.GetAllAsync(); // Agregado abajo si no lo tienes
        var dtos = new List<GroupDto>();

        foreach (var group in groups)
        {
            var posts = await _postRepository.GetByGroupAsync(group.Id);
            var latestPost = posts.OrderByDescending(p => p.CreatedAt).FirstOrDefault(); // CAMBIO AQUÍ

            dtos.Add(new GroupDto
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                AdminId = group.AdminId,
                LatestPost = latestPost == null ? null : new PostDto
                {
                    Id = latestPost.Id,
                    Title = latestPost.Title,
                    Content = latestPost.Content.Text,
                    CreatedAt = latestPost.CreatedAt,
                    AuthorId = latestPost.AuthorId,
                    GroupId = latestPost.GroupId
                }
            });
        }

        return dtos;
    }
}
