using FluentValidation;

namespace ForoUniversitario.ApplicationLayer.Users.Validators;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The name cannot be empty")
            .Length(3, 100).WithMessage("The name must be between 3 and 100 characters")
            .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
            .WithMessage("The name can only contain letters and spaces");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("The email cannot be empty")
            .EmailAddress().WithMessage("It must be a valid email")
            .MaximumLength(150).WithMessage("The email cannot exceed 150 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("The password cannot be empty")
            .MinimumLength(8).WithMessage("The password must be at least 8 characters")
            .Matches(@"[A-Z]").WithMessage("The password must contain at least one uppercase letter")
            .Matches(@"[a-z]").WithMessage("The password must contain at least one lowercase letter")
            .Matches(@"[0-9]").WithMessage("The password must contain at least one number")
            .Matches(@"[\!\?\*\.\@\#\$\%\^\&\+\=\-]").WithMessage("The password must contain at least one special character (! ? * . @ # $ % ^ & + = -)");

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("The specified role is invalid");
    }
}
