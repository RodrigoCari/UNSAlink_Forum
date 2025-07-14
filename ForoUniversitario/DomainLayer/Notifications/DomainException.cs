namespace ForoUniversitario.DomainLayer.Notifications;
public sealed class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}
