using ForoUniversitario.DomainLayer.Groups;
using ForoUniversitario.DomainLayer.Posts;
using ForoUniversitario.DomainLayer.Users;
using Xunit;
using System;

namespace ForoUniversitario.Tests.DomainLayer.Posts;

public class PostTests
{
    private readonly User _author;
    private readonly Group _group;
    private readonly PostContent _content;

    public PostTests()
    {
        _author = new User(Guid.NewGuid(), "Author", "author@email.com", Role.Student, "hash");
        _group = new Group(Guid.NewGuid(), "Group", "Description", _author);
        _content = new PostContent("Test content");
    }

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreatePost()
    {
        var id = Guid.NewGuid();
        var title = "Test Post";
        var type = TypePost.Discussion;

        var post = new Post(id, title, _content, _author.Id, _group.Id, type);

        Assert.Equal(id, post.Id);
        Assert.Equal(title, post.Title);
        Assert.Equal(_content, post.Content);
        Assert.Equal(_author.Id, post.AuthorId);
        Assert.Equal(_group.Id, post.GroupId);
        Assert.Equal(type, post.Type);
    }

    [Fact]
    public void Constructor_WithNullTitle_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Post(Guid.NewGuid(), null!, _content, _author.Id, _group.Id, TypePost.Discussion));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyOrWhitespaceTitle_ShouldThrowArgumentException(string invalidTitle)
    {
        // THIS MUST FAIL IN RED PHASE
        Assert.Throws<ArgumentException>(() =>
            new Post(Guid.NewGuid(), invalidTitle, _content, _author.Id, _group.Id, TypePost.Discussion));
    }

    [Fact]
    public void Constructor_WithHugeTitle_ShouldThrowArgumentException()
    {
        var hugeTitle = new string('a', 101);
        Assert.Throws<ArgumentException>(() =>
            new Post(Guid.NewGuid(), hugeTitle, _content, _author.Id, _group.Id, TypePost.Discussion));
    }

    [Fact]
    public void CreateSharedPost_ShouldCreateSharedPostWithOriginalContent()
    {
        var originalPost = new Post(Guid.NewGuid(), "Original", _content, _author.Id, _group.Id, TypePost.Discussion);
        var newTitle = "Shared: Original";

        var sharedPost = Post.CreateSharedPost(newTitle, Guid.NewGuid(), Guid.NewGuid(), originalPost);

        Assert.Equal(newTitle, sharedPost.Title);
        Assert.Equal(originalPost.Content.Text, sharedPost.Content.Text);
        Assert.Equal(TypePost.Shared, sharedPost.Type);
        Assert.Equal(originalPost.Id, sharedPost.SharedPostId);
    }
}
