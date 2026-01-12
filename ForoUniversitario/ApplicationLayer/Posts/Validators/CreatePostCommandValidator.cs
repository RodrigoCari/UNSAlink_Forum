using FluentValidation;

namespace ForoUniversitario.ApplicationLayer.Posts.Validators;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("The title cannot be empty")
            .Length(5, 100).WithMessage("The title must be between 5 and 100 characters");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("The content cannot be empty")
            .MinimumLength(10).WithMessage("The content must be at least 10 characters")
            .MaximumLength(5000).WithMessage("The content cannot exceed 5000 characters");

        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage("A valid author must be specified");

        RuleFor(x => x.GroupId)
            .NotEmpty().WithMessage("A valid group must be specified");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("The post type is invalid");
    }
}