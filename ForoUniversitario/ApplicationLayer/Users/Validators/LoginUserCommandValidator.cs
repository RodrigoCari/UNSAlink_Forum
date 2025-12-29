using FluentValidation;

namespace ForoUniversitario.ApplicationLayer.Users.Validators;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The username cannot be empty");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("The password cannot be empty");
    }
}