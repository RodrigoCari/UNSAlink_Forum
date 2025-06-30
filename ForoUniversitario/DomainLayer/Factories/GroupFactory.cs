using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.DomainLayer.Users;

namespace ForoUniversitario.DomainLayer.Factories;

public class GroupFactory : IGroupFactory
{
    public Group Create(string name, string description, User admin)
    {
        var id = Guid.NewGuid();
        return new Group(id, name, description, admin);
    }
}
