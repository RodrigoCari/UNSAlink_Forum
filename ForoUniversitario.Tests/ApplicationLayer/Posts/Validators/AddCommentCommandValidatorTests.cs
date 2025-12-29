using FluentValidation.TestHelper;
using ForoUniversitario.ApplicationLayer.Posts;
using ForoUniversitario.ApplicationLayer.Posts.Validators;
using Xunit;

namespace ForoUniversitario.Tests.ApplicationLayer.Posts.Validators;

public class AddCommentCommandValidatorTests    
{
    private readonly AddCommentCommandValidator _validator;

    public AddCommentCommandValidatorTests()
    {
        _validator = new AddCommentCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Content_Is_Empty()
    {
        var command = new AddCommentCommand
        {
            Content = "",
            PostId = Guid.NewGuid()
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Fact]
    public void Should_Have_Error_When_Content_Is_Too_Long()
    {
        var command = new AddCommentCommand
        {
            Content = new string('A', 1001),
            PostId = Guid.NewGuid()
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Fact]
    public void Should_Have_Error_When_PostId_Is_Empty()
    {
        var command = new AddCommentCommand
        {
            Content = "Valid comment",
            PostId = Guid.Empty
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PostId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new AddCommentCommand
        {
            Content = "This is a valid comment",
            PostId = Guid.NewGuid()
        };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}