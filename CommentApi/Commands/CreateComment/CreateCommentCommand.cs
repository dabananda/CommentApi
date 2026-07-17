using CommentApi.Common;
using CommentApi.Common.Abstraction;
using System.ComponentModel.DataAnnotations;

namespace CommentApi.Commands.CreateComment
{
    public record CreateCommentCommand : IRequest<Result>
    {
        [Required(ErrorMessage = "PostId is required")]
        public string PostId { get; init; }

        [Required(ErrorMessage = "AuthorName is required")]
        public string AuthorName { get; init; }

        [EmailAddress(ErrorMessage = "AuthorEmail is not a valid email address")]
        public string? AuthorEmail { get; init; }

        [Required(ErrorMessage = "Content is required")]
        [Length(2, 500, ErrorMessage = "Content must be between 2 and 500 characters")]
        public string Content { get; init; }
        public Guid? ParentCommentId { get; init; }
    }
}
