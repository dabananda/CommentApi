using CommentApi.Common;
using CommentApi.Common.Abstraction;
using CommentApi.Repositories;

namespace CommentApi.Features.Comments.CreateComment
{
    public class CreateCommentCommandHandler(ICommentRepository commentRepository) : IRequestHandler<CreateCommentCommand, Result>
    {
        public async Task<Result> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var entity = CommentMapper.ToEntity(request);
            await commentRepository.CreateCommentAsync(entity, cancellationToken);
            return Result.Success();
        }
    }
}
