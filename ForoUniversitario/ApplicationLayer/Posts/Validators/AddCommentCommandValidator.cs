using FluentValidation;

namespace ForoUniversitario.ApplicationLayer.Posts.Validators;

public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("The comment content cannot be empty")
            .Length(1, 1000).WithMessage("The comment must be between 1 and 1000 characters long");

        RuleFor(x => x.PostId)
            .NotEmpty().WithMessage("A valid post must be specified");
    }
}
