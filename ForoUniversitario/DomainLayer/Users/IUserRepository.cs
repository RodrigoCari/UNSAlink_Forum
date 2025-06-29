namespace ForoUniversitario.DomainLayer.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    Task ModifyAsync(User user);
    Task DeleteAsync(Guid id);
}
