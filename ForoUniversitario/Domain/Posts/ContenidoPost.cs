namespace ForoUniversitario.Domain.Posts;

public class ContenidoPost
{
    public string Texto { get; }

    public ContenidoPost(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            throw new ArgumentException("El contenido no puede estar vacío");

        if (texto.Length > 1000)
            throw new ArgumentException("El contenido no puede tener más de 1000 caracteres");

        Texto = texto;
    }

    // Value Object: igualdad por valor
    public override bool Equals(object? obj)
    {
        return obj is ContenidoPost other && Texto == other.Texto;
    }

    public override int GetHashCode() => Texto.GetHashCode();
}
