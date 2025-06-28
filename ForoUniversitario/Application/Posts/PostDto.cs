namespace ForoUniversitario.Application.Posts;

public class PostDto
{
    public Guid Id { get; set; }
    public string Contenido { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
}
