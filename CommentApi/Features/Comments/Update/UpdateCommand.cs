using CommentApi.Common;
using CommentApi.Common.Abstraction;

namespace CommentApi.Features.Comments.Update
{
    public record UpdateCommand(
        Guid Id,
        string PostId,
        string AuthorName,
        string? AuthorEmail,
        string Content,
        string? ParentCommentId) : IRequest<Result>;
}
