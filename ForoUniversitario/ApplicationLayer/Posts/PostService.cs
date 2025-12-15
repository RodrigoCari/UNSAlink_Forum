using ForoUniversitario.DomainLayer.Posts;
using ForoUniversitario.DomainLayer.Users;
using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.DomainLayer.Factories;
using ForoUniversitario.DomainLayer.DomainServices;

namespace ForoUniversitario.ApplicationLayer.Posts;

public class PostService : IPostService
{
    private const string UnknownName = "Unknown";

    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IPostFactory _postFactory;
    private readonly IPostDomainService _postDomainService;

    public PostService(
        IPostRepository postRepository,
        IUserRepository userRepository,
        IGroupRepository groupRepository,
        IPostFactory postFactory,
        IPostDomainService postDomainService)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        _postFactory = postFactory;
        _postDomainService = postDomainService;
    }

    public async Task<Guid> CreateAsync(CreatePostCommand command)
    {
        var post = _postFactory.CreatePost(
            command.Title,
            command.Content,
            command.AuthorId,
            command.GroupId,
            command.Type);

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
            AuthorName = author?.Name ?? UnknownName,
            GroupId = post.GroupId,
            GroupName = group?.Name ?? UnknownName,
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
                AuthorName = author?.Name ?? UnknownName,
                GroupId = post.GroupId,
                GroupName = group?.Name ?? UnknownName,
                Type = post.Type,
                CreatedAt = post.CreatedAt
            });
        }

        return result;
    }

    public async Task<IEnumerable<PostDto>> GetByGroupAsync(Guid groupId)
    {
        var posts = await _postRepository.GetByGroupAsync(groupId);
        var result = new List<PostDto>();

        foreach (var post in posts)
        {
            var author = await _userRepository.GetByIdAsync(post.AuthorId);
            var group = await _groupRepository.FindAsync(post.GroupId);

            PostDto? sharedPostDto = null;

            if (post.SharedPostId.HasValue)
            {
                var sharedPost = await _postRepository.GetByIdAsync(post.SharedPostId.Value);
                if (sharedPost != null)
                {
                    var sharedAuthor = await _userRepository.GetByIdAsync(sharedPost.AuthorId);
                    var sharedGroup = await _groupRepository.FindAsync(sharedPost.GroupId);

                    sharedPostDto = new PostDto
                    {
                        Id = sharedPost.Id,
                        Title = sharedPost.Title,
                        Content = sharedPost.Content.Text,
                        AuthorId = sharedPost.AuthorId,
                        AuthorName = sharedAuthor?.Name ?? UnknownName,
                        GroupId = sharedPost.GroupId,
                        GroupName = sharedGroup?.Name ?? UnknownName,
                        Type = sharedPost.Type,
                        CreatedAt = sharedPost.CreatedAt
                    };
                }
            }

            result.Add(new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content.Text,
                AuthorId = post.AuthorId,
                AuthorName = author?.Name ?? UnknownName,
                GroupId = post.GroupId,
                GroupName = group?.Name ?? UnknownName,
                Type = post.Type,
                CreatedAt = post.CreatedAt,
                SharedPost = sharedPostDto,
                Comments = post.Comments.Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    Author = c.Author,
                    CreatedAt = c.CreatedAt
                }).ToList()
            });
        }

        return result;
    }

    public async Task ShareToGroupAsync(Guid postId, Guid groupId)
    {
        var post = await _postRepository.GetByIdAsync(postId);
        if (post == null)
            throw new KeyNotFoundException("Post no encontrado");

        bool canShare = await _postDomainService.CanSharePostToGroupAsync(post, groupId);
        if (!canShare)
            throw new InvalidOperationException("No se puede compartir el post en el grupo destino");

        throw new NotImplementedException("Implementa la lógica para compartir el post en el grupo");
    }

    public Task RequestIdeasAsync(Guid postId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<PostDto>> GetPostsByUserAsync(Guid userId)
    {
        var posts = await _postRepository.GetPostsByUserAsync(userId);

        return posts.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Content = p.Content.Text,
            AuthorId = p.AuthorId,
            GroupId = p.GroupId,
            CreatedAt = p.CreatedAt,
            Type = p.Type
        });
    }

    public async Task<Guid> ShareAsync(SharePostCommand command)
    {
        var originalPost = await _postRepository.GetByIdAsync(command.OriginalPostId);
        if (originalPost == null)
            throw new KeyNotFoundException("Post original no encontrado");

        var content = new PostContent(originalPost.Content.Text);

        var sharedPost = new Post(
            Guid.NewGuid(),
            command.Title,
            content,
            command.AuthorId,
            command.GroupId,
            TypePost.Shared)
        {
            SharedPostId = command.OriginalPostId
        };

        await _postRepository.AddAsync(sharedPost);
        await _postRepository.SaveChangesAsync();

        return sharedPost.Id;
    }
}
