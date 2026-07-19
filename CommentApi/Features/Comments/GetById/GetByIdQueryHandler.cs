using CommentApi.Common;
using CommentApi.Common.Abstraction;
using CommentApi.Repositories;

namespace CommentApi.Features.Comments.GetById
{
    public class GetByIdQueryHandler(ICommentRepository commentRepository) : IRequestHandler<GetByIdQuery, Result<CommentDto>>
    {
        public async Task<Result<CommentDto>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var comment = await commentRepository.GetCommentByIdAsync(request.Id, cancellationToken);

            if (comment is null)
                return Result<CommentDto>.Failure("No comment found");

            return Result<CommentDto>.Success(comment.ToDto());
        }
    }
}
