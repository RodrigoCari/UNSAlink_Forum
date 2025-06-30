using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.DomainLayer.Users;

namespace ForoUniversitario.DomainLayer.DomainServices;

public class GroupDomainService : IGroupDomainService
{
    public async Task AddMemberAsync(Group group, User user)
    {
        if (group.Members.Any(m => m.Id == user.Id))
            return;

        group.Members.Add(user);
        await Task.CompletedTask;
    }

    public async Task RemoveMemberAsync(Group group, User user)
    {
        var member = group.Members.FirstOrDefault(m => m.Id == user.Id);
        if (member != null)
            group.Members.Remove(member);

        await Task.CompletedTask;
    }

    public bool CanUserViewGroup(Group group, User user)
    {
        if (group.AdminId == user.Id) return true;
        if (group.Members.Any(m => m.Id == user.Id)) return true;
        if (group.Viewers.Any(v => v.Id == user.Id)) return true;

        return false;
    }
}
