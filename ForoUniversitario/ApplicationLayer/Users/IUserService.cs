namespace ForoUniversitario.ApplicationLayer.Users;

public interface IUserService
{
    Task<Guid> RegisterAsync(RegisterUserCommand command);
    Task UpdateProfileAsync(Guid id, UpdateUserProfileCommand command);
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<List<string>> GetWorksAsync(Guid id);
    Task<string> LoginAsync(LoginUserCommand command);
}
