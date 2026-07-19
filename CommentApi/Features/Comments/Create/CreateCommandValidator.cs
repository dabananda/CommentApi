using FluentValidation;

namespace CommentApi.Features.Comments.Create
{
    public class CreateCommandValidator : AbstractValidator<CreateCommand>
    {
        public CreateCommandValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("PostId is required");

            RuleFor(x => x.AuthorName)
                .NotEmpty().WithMessage("AuthorName is required");

            RuleFor(x => x.AuthorEmail)
                .EmailAddress().WithMessage("Invalid email address")
                .When(x => !string.IsNullOrEmpty(x.AuthorEmail));

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required")
                .Length(2, 500).WithMessage("Content must be between 2 and 500 characters");
        }
    }
}
