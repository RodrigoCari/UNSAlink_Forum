using ForoUniversitario.ApplicationLayer.Groups;
using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.DomainLayer.Users;
using Xunit;

namespace ForoUniversitario.Tests.ApplicationLayer.Groups;

public class GroupDtoMapperTests
{
    [Fact]
    public void Map_Should_Map_Group_To_GroupDto_Correctly()
    {
        // Arrange
        var admin = new User(
            Guid.NewGuid(),
            "Test User",
            "test@email.com",
            Role.Administrative,
            "hash"
        );

        var group = new Group(
            Guid.NewGuid(),
            "Test Group",
            "Description",
            admin
        );

        var mapper = new GroupDtoMapper();

        // Act
        var dto = mapper.Map(group);

        // Assert
        Assert.Equal(group.Id, dto.Id);
        Assert.Equal(group.Name, dto.Name);
        Assert.Equal(group.Description, dto.Description);
        Assert.Equal(group.AdminId, dto.AdminId);
        Assert.Null(dto.LatestPost);
    }
}
