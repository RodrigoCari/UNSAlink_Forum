using ForoUniversitario.Domain.Posts;

namespace ForoUniversitario.Application.Posts;

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
        var post = new Post(Guid.NewGuid(), command.Title, content, command.Author);

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
            CreatedAt = post.CreatedAt
        };
    }
}
