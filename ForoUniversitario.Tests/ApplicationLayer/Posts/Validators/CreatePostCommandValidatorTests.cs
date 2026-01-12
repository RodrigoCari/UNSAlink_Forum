using FluentValidation.TestHelper;
using ForoUniversitario.ApplicationLayer.Posts;
using ForoUniversitario.ApplicationLayer.Posts.Validators;
using ForoUniversitario.DomainLayer.Posts;
using Xunit;

namespace ForoUniversitario.Tests.ApplicationLayer.Posts.Validators;

public class CreatePostCommandValidatorTests
{
    private readonly CreatePostCommandValidator _validator;

    public CreatePostCommandValidatorTests()
    {
        _validator = new CreatePostCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var command = new CreatePostCommand
        {
            Title = "",
            Content = "Valid content with enough characters",
            AuthorId = Guid.NewGuid(),
            GroupId = Guid.NewGuid(),
            Type = TypePost.Discussion
        };  
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("The title cannot be empty");
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Too_Short()
    {
        var command = new CreatePostCommand
        {
            Title = "Abc",
            Content = "Valid content",
            AuthorId = Guid.NewGuid(),
            GroupId = Guid.NewGuid(),
            Type = TypePost.Discussion
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Too_Long()
    {
        var command = new CreatePostCommand
        {
            Title = new string('A', 101),
            Content = "Valid content with enough characters",
            AuthorId = Guid.NewGuid(),
            GroupId = Guid.NewGuid(),
            Type = TypePost.Discussion
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Have_Error_When_Content_Is_Empty()
    {
        var command = new CreatePostCommand
        {
            Title = "Valid Title",
            Content = "",
            AuthorId = Guid.NewGuid(),
            GroupId = Guid.NewGuid(),
            Type = TypePost.Discussion
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Fact]
    public void Should_Have_Error_When_Content_Is_Too_Short()
    {
        var command = new CreatePostCommand
        {
            Title = "Valid Title",
            Content = "Short",
            AuthorId = Guid.NewGuid(),
            GroupId = Guid.NewGuid(),
            Type = TypePost.Discussion
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Fact]
    public void Should_Have_Error_When_AuthorId_Is_Empty()
    {
        var command = new CreatePostCommand
        {
            Title = "Valid Title",
            Content = "Valid content with enough characters",
            AuthorId = Guid.Empty,
            GroupId = Guid.NewGuid(),
            Type = TypePost.Discussion
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.AuthorId);
    }

    [Fact]
    public void Should_Have_Error_When_GroupId_Is_Empty()
    {
        var command = new CreatePostCommand
        {
            Title = "Valid Title",
            Content = "Valid content with enough characters",
            AuthorId = Guid.NewGuid(),
            GroupId = Guid.Empty,
            Type = TypePost.Discussion
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.GroupId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new CreatePostCommand
        {
            Title = "Valid Title",
            Content = "This is a valid content with more than 10 characters",
            AuthorId = Guid.NewGuid(),
            GroupId = Guid.NewGuid(),
            Type = TypePost.Discussion
        };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}