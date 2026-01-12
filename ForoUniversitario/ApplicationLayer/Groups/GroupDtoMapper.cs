using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.DomainLayer.Posts;
using ForoUniversitario.ApplicationLayer.Posts;

namespace ForoUniversitario.ApplicationLayer.Groups;

public class GroupDtoMapper : IGroupDtoMapper
{
    public GroupDto Map(Group group)
    {
        return new GroupDto
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            AdminId = group.AdminId
        };
    }

    public GroupDto MapWithLatestPost(Group group, Post? latestPost)
    {
        var dto = Map(group);

        if (latestPost == null)
            return dto;

        dto.LatestPost = new PostDto
        {
            Id = latestPost.Id,
            Title = latestPost.Title,
            Content = latestPost.Type == TypePost.Shared && latestPost.SharedPost != null
                ? latestPost.SharedPost.Content.Text
                : latestPost.Content.Text,
            CreatedAt = latestPost.CreatedAt,
            AuthorId = latestPost.AuthorId,
            GroupId = latestPost.GroupId
        };

        return dto;
    }
}

