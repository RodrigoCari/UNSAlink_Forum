using FluentValidation;

namespace ForoUniversitario.ApplicationLayer.Users.Validators;

public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileCommandValidator()
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

        RuleFor(x => x.Interests)
            .NotNull().WithMessage("The interests list cannot be null");

        RuleForEach(x => x.Interests)
            .NotEmpty().WithMessage("Interests cannot be empty")
            .MaximumLength(50).WithMessage("Each interest cannot exceed 50 characters");
    }
}