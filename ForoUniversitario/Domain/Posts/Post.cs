namespace ForoUniversitario.Domain.Posts;

public class Post
{
    public Guid Id { get; private set; }
    public ContenidoPost Contenido { get; private set; }
    public DateTime FechaCreacion { get; private set; }

    private Post() { } // Constructor privado para EF

    public Post(Guid id, ContenidoPost contenido)
    {
        Id = id;
        Contenido = contenido ?? throw new ArgumentNullException(nameof(contenido));
        FechaCreacion = DateTime.UtcNow;
    }

    public void EditarContenido(string nuevoTexto)
    {
        Contenido = new ContenidoPost(nuevoTexto);
    }
}
