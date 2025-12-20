namespace ForoUniversitario.ApplicationLayer.Security;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? Name { get; }
    bool IsAuthenticated { get; }
}
