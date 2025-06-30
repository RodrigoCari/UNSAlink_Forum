using ForoUniversitario.DomainLayer.Posts;
using ForoUniversitario.DomainLayer.Users;
using ForoUniversitario.DomainLayer.Groups;

namespace ForoUniversitario.ApplicationLayer.Posts;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;

    public PostService(
        IPostRepository postRepository,
        IUserRepository userRepository,
        IGroupRepository groupRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _groupRepository = groupRepository;
    }

    public async Task<Guid> CreateAsync(CreatePostCommand command)
    {
        var post = new Post(
            Guid.NewGuid(),
            command.Title,
            new PostContent(command.Content),
            command.AuthorId,
            command.GroupId,
            command.Type
        );

        await _postRepository.AddAsync(post);
        await _postRepository.SaveChangesAsync();
        return post.Id;
    }

    public async Task<PostDto?> GetByIdAsync(Guid id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null) return null;

        var author = await _userRepository.GetByIdAsync(post.AuthorId);
        var group = await _groupRepository.FindAsync(post.GroupId);

        return new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content.Text,
            AuthorId = post.AuthorId,
            AuthorName = author?.Name ?? "Unknown",
            GroupId = post.GroupId,
            GroupName = group?.Name ?? "Unknown",
            Type = post.Type,
            CreatedAt = post.CreatedAt
        };
    }

    public async Task<IEnumerable<PostDto>> GetByTypeAsync(int typeInt)
    {
        var type = (TypePost)typeInt;
        var posts = await _postRepository.GetByTypeAsync(type);

        var result = new List<PostDto>();
        foreach (var post in posts)
        {
            var author = await _userRepository.GetByIdAsync(post.AuthorId);
            var group = await _groupRepository.FindAsync(post.GroupId);

            result.Add(new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content.Text,
                AuthorId = post.AuthorId,
                AuthorName = author?.Name ?? "Unknown",
                GroupId = post.GroupId,
                GroupName = group?.Name ?? "Unknown",
                Type = post.Type,
                CreatedAt = post.CreatedAt
            });
        }

        return result;
    }

    public Task ShareToGroupAsync(Guid postId, Guid groupId)
    {
        // Lógica opcional que podrías implementar.
        throw new NotImplementedException();
    }

    public Task RequestIdeasAsync(Guid postId)
    {
        // Lógica opcional que podrías implementar.
        throw new NotImplementedException();
    }
}
