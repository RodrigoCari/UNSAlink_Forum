namespace ForoUniversitario.Application.Posts;

public interface IPostService
{
    Task<Guid> CrearPostAsync(CreatePostCommand command);
    Task<PostDto?> ObtenerPostPorIdAsync(Guid id);
}
