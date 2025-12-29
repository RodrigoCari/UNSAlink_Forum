using FluentValidation;

namespace ForoUniversitario.ApplicationLayer.Groups.Validators;

public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
{
    public CreateGroupCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The group name cannot be empty")
            .Length(3, 50).WithMessage("The name must be between 3 and 50 characters long")
            .Matches(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\-_]+$")
            .WithMessage("The name can only contain letters, numbers, spaces, hyphens, and underscores");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("The description cannot be empty")
            .MaximumLength(500).WithMessage("The description cannot exceed 500 characters");

        RuleFor(x => x.AdminId)
            .NotEmpty().WithMessage("A valid administrator must be specified");
    }
}
