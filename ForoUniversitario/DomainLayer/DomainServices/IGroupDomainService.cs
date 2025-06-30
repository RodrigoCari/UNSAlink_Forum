using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.DomainLayer.Users;

namespace ForoUniversitario.DomainLayer.DomainServices;

public interface IGroupDomainService
{
    Task AddMemberAsync(Group group, User user);
    Task RemoveMemberAsync(Group group, User user);
    bool CanUserViewGroup(Group group, User user);
}
