using CommentApi.Common;
using CommentApi.Common.Abstraction;

namespace CommentApi.Features.Comments.GetById
{
    public record GetByIdQuery(Guid Id) : IRequest<Result<CommentDto>>;
}
