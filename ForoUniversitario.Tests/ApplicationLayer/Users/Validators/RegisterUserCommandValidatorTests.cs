using FluentValidation.TestHelper;
using ForoUniversitario.ApplicationLayer.Users;
using ForoUniversitario.ApplicationLayer.Users.Validators;
using ForoUniversitario.DomainLayer.Users;
using Xunit;

namespace ForoUniversitario.Tests.ApplicationLayer.Users.Validators;

public class RegisterUserCommandValidatorTests
{
    private readonly RegisterUserCommandValidator _validator;

    public RegisterUserCommandValidatorTests()
    {
        _validator = new RegisterUserCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new RegisterUserCommand
        {
            Name = "",
            Email = "valid@email.com",
            Password = "ValidPass1!",
            Role = Role.Student
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Too_Short()
    {
        var command = new RegisterUserCommand
        {
            Name = "Ab",
            Email = "valid@email.com",
            Password = "ValidPass1!",
            Role = Role.Student
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Contains_Numbers()
    {
        var command = new RegisterUserCommand
        {
            Name = "John123",
            Email = "valid@email.com",
            Password = "ValidPass1!",
            Role = Role.Student
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var command = new RegisterUserCommand
        {
            Name = "John Doe",
            Email = "invalid-email",
            Password = "ValidPass1!",
            Role = Role.Student
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Too_Short()
    {
        var command = new RegisterUserCommand
        {
            Name = "John Doe",
            Email = "valid@email.com",
            Password = "Short1!",
            Role = Role.Student
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Has_No_Uppercase()
    {
        var command = new RegisterUserCommand
        {
            Name = "John Doe",
            Email = "valid@email.com",
            Password = "password1!",
            Role = Role.Student
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Has_No_Number()
    {
        var command = new RegisterUserCommand
        {
            Name = "John Doe",
            Email = "valid@email.com",
            Password = "Password!",
            Role = Role.Student
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Has_No_Special_Character()
    {
        var command = new RegisterUserCommand
        {
            Name = "John Doe",
            Email = "valid@email.com",
            Password = "Password1",
            Role = Role.Student
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new RegisterUserCommand
        {
            Name = "John Doe",
            Email = "john@example.com",
            Password = "Password1!",
            Role = Role.Student
        };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}