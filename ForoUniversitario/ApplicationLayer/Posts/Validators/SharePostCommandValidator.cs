using FluentValidation;

namespace ForoUniversitario.ApplicationLayer.Posts.Validators;

public class SharePostCommandValidator : AbstractValidator<SharePostCommand>
{
    public SharePostCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("The title can not be empty")
            .Length(5, 100).WithMessage("The title must be between 5 and 100 characters");

        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage("A valid author must be specified");

        RuleFor(x => x.GroupId)
            .NotEmpty().WithMessage("A valid group must be specified");

        RuleFor(x => x.OriginalPostId)
            .NotEmpty().WithMessage("The original post to share must be specified");
    }
}