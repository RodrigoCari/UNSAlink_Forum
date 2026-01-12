using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.DomainLayer.Posts;
using ForoUniversitario.ApplicationLayer.Posts;

namespace ForoUniversitario.ApplicationLayer.Groups
{
    public interface IGroupDtoMapper
    {
        GroupDto Map(Group group);
        GroupDto MapWithLatestPost(Group group, Post? latestPost);
    }
}
