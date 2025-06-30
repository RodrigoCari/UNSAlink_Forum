using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.DomainLayer.Users;

namespace ForoUniversitario.DomainLayer.Factories;

public interface IGroupFactory
{
    Group Create(string name, string description, User admin);
}
