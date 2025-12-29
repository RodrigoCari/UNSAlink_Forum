using FluentValidation.TestHelper;
using ForoUniversitario.ApplicationLayer.Notifications;
using ForoUniversitario.ApplicationLayer.Notifications.Validators;
using ForoUniversitario.DomainLayer.Notifications;
using Xunit;

namespace ForoUniversitario.Tests.ApplicationLayer.Notifications.Validators;

public class SendNotificationCommandValidatorTests
{
    private readonly SendNotificationCommandValidator _validator;

    public SendNotificationCommandValidatorTests()
    {
        _validator = new SendNotificationCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_ReceiverId_Is_Empty()
    {
        var command = new SendNotificationCommand
        {
            ReceiverId = Guid.Empty,
            Message = "Valid message",
            NotificationType = TypeNotification.NewComment
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ReceiverId);
    }

    [Fact]
    public void Should_Have_Error_When_Message_Is_Empty()
    {
        var command = new SendNotificationCommand
        {
            ReceiverId = Guid.NewGuid(),
            Message = "",
            NotificationType = TypeNotification.NewComment
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Message);
    }

    [Fact]
    public void Should_Have_Error_When_Message_Is_Too_Long()
    {
        var command = new SendNotificationCommand
        {
            ReceiverId = Guid.NewGuid(),
            Message = new string('A', 501),
            NotificationType = TypeNotification.NewComment
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Message);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new SendNotificationCommand
        {
            ReceiverId = Guid.NewGuid(),
            Message = "This is a valid notification message",
            NotificationType = TypeNotification.NewComment
        };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}