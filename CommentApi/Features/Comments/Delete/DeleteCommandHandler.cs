using CommentApi.Common;
using CommentApi.Common.Abstraction;
using CommentApi.Repositories;

namespace CommentApi.Features.Comments.Delete
{
    public class DeleteCommandHandler(ICommentRepository commentRepository) : IRequestHandler<DeleteCommand, Result>
    {
        public async Task<Result> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var comment = await commentRepository.GetCommentByIdAsync(request.Id, cancellationToken);
            
            if (comment is null)
                return Result.Failure("No comment found");
            
            await commentRepository.DeleteCommentAsync(comment, cancellationToken);
            
            return Result.Success("Comment deleted successfully");
        }
    }
}
