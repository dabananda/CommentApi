using CommentApi.Common;
using CommentApi.Common.Abstraction;

namespace CommentApi.Features.Comments.Delete
{
    public record DeleteCommand(Guid Id) : IRequest<Result>;
}
