using ForoUniversitario.DomainLayer.Posts;
using ForoUniversitario.DomainLayer.Users;
using ForoUniversitario.DomainLayer.Groups;
using Xunit;

namespace ForoUniversitario.Tests.DomainLayer.Users;

public class UserTests
{
    [Fact]
    public void AddPost_ShouldAddPostToCollection()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "Test User", "test@email.com", Role.Student, "hash");
        var post = new Post(Guid.NewGuid(), "Title", new PostContent("Content"), user.Id, Guid.NewGuid(), TypePost.Discussion); // Correcting assumption

        // Act
        user.AddPost(post); // This method does not exist yet (RED)

        // Assert
        Assert.Contains(post, user.Posts);
        Assert.Single(user.Posts);
    }
}
