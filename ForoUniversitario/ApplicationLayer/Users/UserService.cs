using ForoUniversitario.DomainLayer.Users;

namespace ForoUniversitario.ApplicationLayer.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> RegisterAsync(RegisterUserCommand command)
    {
        var user = new User(Guid.NewGuid(), command.Name, command.Email, command.Role);
        await _repository.AddAsync(user);
        return user.Id;
    }

    public async Task UpdateProfileAsync(Guid id, UpdateUserProfileCommand command)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) throw new Exception("User not found");

        user.UpdateProfile(command.Name, command.Email);
        await _repository.ModifyAsync(user);
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        return user == null ? null : new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    public async Task<List<string>> GetWorksAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) throw new Exception("User not found");

        return new List<string> { "Work A", "Work B", "Work C" };
    }
}
