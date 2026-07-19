using CommentApi.Common;
using CommentApi.Common.Abstraction;

namespace CommentApi.Features.Comments.GetAll
{
    public record GetAllQuery() : IRequest<Result<IEnumerable<CommentDto>>>;
}
