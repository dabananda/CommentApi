using CommentApi.Common;
using CommentApi.Common.Abstraction;
using CommentApi.Repositories;

namespace CommentApi.Features.Comments.Create
{
    public class CreateCommandHandler(ICommentRepository commentRepository) : IRequestHandler<CreateCommand, Result>
    {
        public async Task<Result> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            var entity = CommentMapper.ToEntity(request);
            await commentRepository.CreateCommentAsync(entity, cancellationToken);
            return Result.Success();
        }
    }
}
