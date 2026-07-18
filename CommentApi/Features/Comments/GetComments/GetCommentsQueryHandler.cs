using CommentApi.Common;
using CommentApi.Common.Abstraction;
using CommentApi.Repositories;

namespace CommentApi.Features.Comments.GetComments
{
    public class GetCommentsQueryHandler(ICommentRepository commentRepository) : IRequestHandler<GetCommentsQuery, Result<IEnumerable<CommentDto>>>
    {
        public async Task<Result<IEnumerable<CommentDto>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
        {
            var comments = await commentRepository.GetCommentsAsync(cancellationToken);
            var commentDtos = comments.Select(CommentMapper.ToDto);
            return Result<IEnumerable<CommentDto>>.Success(commentDtos);
        }
    }
}
