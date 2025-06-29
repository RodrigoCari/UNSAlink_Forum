namespace ForoUniversitario.Domain.Posts;

public class PostContent
{
    public string Text { get; }

    public PostContent(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Content cannot be empty");

        if (text.Length > 1000)
            throw new ArgumentException("Content cannot exceed 1000 characters");

        Text = text;
    }

    public override bool Equals(object? obj)
    {
        return obj is PostContent other && Text == other.Text;
    }

    public override int GetHashCode() => Text.GetHashCode();
}
