using FluentValidation.TestHelper;
using ForoUniversitario.ApplicationLayer.Groups;
using ForoUniversitario.ApplicationLayer.Groups.Validators;
using Xunit;

namespace ForoUniversitario.Tests.ApplicationLayer.Groups.Validators;

public class CreateGroupCommandValidatorTests
{
    private readonly CreateGroupCommandValidator _validator;

    public CreateGroupCommandValidatorTests()
    {
        _validator = new CreateGroupCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new CreateGroupCommand { Name = "" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Too_Short()
    {
        var command = new CreateGroupCommand { Name = "AB" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Contains_Invalid_Characters()
    {
        var command = new CreateGroupCommand { Name = "Group@Name!" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var command = new CreateGroupCommand { Description = "" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Too_Long()
    {
        var command = new CreateGroupCommand { Description = new string('A', 501) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_AdminId_Is_Empty()
    {
        var command = new CreateGroupCommand { AdminId = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.AdminId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new CreateGroupCommand
        {
            Name = "Valid Group Name",
            Description = "This is a valid description",
            AdminId = Guid.NewGuid()
        };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}