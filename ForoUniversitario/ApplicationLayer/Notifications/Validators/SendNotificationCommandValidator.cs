using FluentValidation;

namespace ForoUniversitario.ApplicationLayer.Notifications.Validators;

public class SendNotificationCommandValidator : AbstractValidator<SendNotificationCommand>
{
    public SendNotificationCommandValidator()
    {
        RuleFor(x => x.ReceiverId)
            .NotEmpty().WithMessage("A valid recipient must be specified");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("The message cannot be empty")
            .MaximumLength(500).WithMessage("The message cannot exceed 500 characters");

        RuleFor(x => x.NotificationType)
            .IsInEnum().WithMessage("The notification type is invalid");
    }
}