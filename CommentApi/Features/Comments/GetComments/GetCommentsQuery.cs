using CommentApi.Common;
using CommentApi.Common.Abstraction;

namespace CommentApi.Features.Comments.GetComments
{
    public record GetCommentsQuery() : IRequest<Result<IEnumerable<CommentDto>>>;
}
