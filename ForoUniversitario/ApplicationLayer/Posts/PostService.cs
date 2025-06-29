using ForoUniversitario.DomainLayer.Posts;

namespace ForoUniversitario.ApplicationLayer.Posts;

public class PostService : IPostService
{
    private readonly IPostRepository _repository;

    public PostService(IPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateAsync(CreatePostCommand command)
    {
        var content = new PostContent(command.Content);
        var post = new Post(Guid.NewGuid(), command.Title, content, command.Author, command.Type);

        await _repository.AddAsync(post);
        await _repository.SaveChangesAsync();

        return post.Id;
    }

    public async Task<PostDto?> GetByIdAsync(Guid id)
    {
        var post = await _repository.GetByIdAsync(id);
        if (post == null) return null;

        return new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content.Text,
            Author = post.Author,
            Type = post.Type.ToString(),
            CreatedAt = post.CreatedAt
        };
    }

    // Share a group (solo simula por ahora)
    public async Task ShareToGroupAsync(Guid postId, Guid groupId)
    {
        var post = await _repository.GetByIdAsync(postId);
        if (post == null) throw new InvalidOperationException("Post not found.");

        // Lógica ficticia de compartir (se puede expandir luego)
        Console.WriteLine($"Post {postId} shared to group {groupId}.");
    }

    // Request Ideas (solo simula por ahora)
    public async Task RequestIdeasAsync(Guid postId)
    {
        var post = await _repository.GetByIdAsync(postId);
        if (post == null) throw new InvalidOperationException("Post not found.");

        // Simulación de lógica de petición de ideas
        Console.WriteLine($"Ideas requested for post {postId}.");
    }

    // Get by Type
    public async Task<IEnumerable<PostDto>> GetByTypeAsync(int type)
    {
        if (!Enum.IsDefined(typeof(TypePost), type))
            throw new ArgumentException("Invalid type value.");

        var posts = await _repository.GetByTypeAsync((TypePost)type);

        return posts.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Content = p.Content.Text,
            Author = p.Author,
            Type = p.Type.ToString(),
            CreatedAt = p.CreatedAt
        });
    }
}
