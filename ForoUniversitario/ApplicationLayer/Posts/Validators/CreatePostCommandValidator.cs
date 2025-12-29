using FluentValidation;

namespace ForoUniversitario.ApplicationLayer.Posts.Validators;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title must not be empty")
            .Length(5, 100).WithMessage("Title must be between 5 and 100 characters");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content must not be empty")
            .MinimumLength(10).WithMessage("Content must be at least 10 characters long")
            .MaximumLength(5000).WithMessage("Content must not exceed 5000 characters");

        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage("A valid author must be specified");

        RuleFor(x => x.GroupId)
            .NotEmpty().WithMessage("A valid group must be specified");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid post type");
    }
}