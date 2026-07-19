using CommentApi.Common;
using CommentApi.Common.Abstraction;
using CommentApi.Repositories;

namespace CommentApi.Features.Comments.GetAll
{
    public class GetAllQueryHandler(ICommentRepository commentRepository) : IRequestHandler<GetAllQuery, Result<IEnumerable<CommentDto>>>
    {
        public async Task<Result<IEnumerable<CommentDto>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var comments = await commentRepository.GetCommentsAsync(cancellationToken);
            var commentDtos = comments.Select(CommentMapper.ToDto);
            return Result<IEnumerable<CommentDto>>.Success(commentDtos);
        }
    }
}
