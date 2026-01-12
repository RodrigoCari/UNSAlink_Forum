using FluentValidation.TestHelper;
using ForoUniversitario.ApplicationLayer.Posts;
using ForoUniversitario.ApplicationLayer.Posts.Validators;
using Xunit;

namespace ForoUniversitario.Tests.ApplicationLayer.Posts.Validators;

public class SharePostCommandValidatorTests
{
    private readonly SharePostCommandValidator _validator;

    public SharePostCommandValidatorTests()
    {
        _validator = new SharePostCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var command = new SharePostCommand
        {
            Title = "",
            AuthorId = Guid.NewGuid(),
            GroupId = Guid.NewGuid(),
            OriginalPostId = Guid.NewGuid()
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Too_Short()
    {
        var command = new SharePostCommand
        {
            Title = "ABC",
            AuthorId = Guid.NewGuid(),
            GroupId = Guid.NewGuid(),
            OriginalPostId = Guid.NewGuid()
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Have_Error_When_AuthorId_Is_Empty()
    {
        var command = new SharePostCommand
        {
            Title = "Valid Title",
            AuthorId = Guid.Empty,
            GroupId = Guid.NewGuid(),
            OriginalPostId = Guid.NewGuid()
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.AuthorId);
    }

    [Fact]
    public void Should_Have_Error_When_GroupId_Is_Empty()
    {
        var command = new SharePostCommand
        {
            Title = "Valid Title",
            AuthorId = Guid.NewGuid(),
            GroupId = Guid.Empty,
            OriginalPostId = Guid.NewGuid()
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.GroupId);
    }

    [Fact]
    public void Should_Have_Error_When_OriginalPostId_Is_Empty()
    {
        var command = new SharePostCommand
        {
            Title = "Valid Title",
            AuthorId = Guid.NewGuid(),
            GroupId = Guid.NewGuid(),
            OriginalPostId = Guid.Empty
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.OriginalPostId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new SharePostCommand
        {
            Title = "Valid Shared Post Title",
            AuthorId = Guid.NewGuid(),
            GroupId = Guid.NewGuid(),
            OriginalPostId = Guid.NewGuid()
        };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}