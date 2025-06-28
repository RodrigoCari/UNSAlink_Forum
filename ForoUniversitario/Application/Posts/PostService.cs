using ForoUniversitario.Domain.Posts;

namespace ForoUniversitario.Application.Posts;

public class PostService : IPostService
{
    private readonly IPostRepository _repository;

    public PostService(IPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CrearPostAsync(CreatePostCommand command)
    {
        var contenido = new ContenidoPost(command.Contenido);
        var post = new Post(Guid.NewGuid(), contenido);

        await _repository.AddAsync(post);
        await _repository.SaveChangesAsync();

        return post.Id;
    }

    public async Task<PostDto?> ObtenerPostPorIdAsync(Guid id)
    {
        var post = await _repository.GetByIdAsync(id);

        if (post == null)
            return null;

        return new PostDto
        {
            Id = post.Id,
            Contenido = post.Contenido.Texto,
            FechaCreacion = post.FechaCreacion
        };
    }
}
