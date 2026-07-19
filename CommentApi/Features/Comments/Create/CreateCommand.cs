using CommentApi.Common;
using CommentApi.Common.Abstraction;

namespace CommentApi.Features.Comments.Create
{
    public record CreateCommand : IRequest<Result>
    {
        public string PostId { get; init; }
        public string AuthorName { get; init; }
        public string? AuthorEmail { get; init; }
        public string Content { get; init; }
        public Guid? ParentCommentId { get; init; }
    }
}
